using Leopotam.Ecs;
using UnityEngine;

/// <summary>
/// System for processing purchases (levels and upgrades).
/// Processes purchase events and checks for sufficient funds.
/// Uses direct links to entity for O(1) access.
/// </summary>
sealed class PurchaseSystem : IEcsRunSystem {
    EcsFilter<PurchaseLevelEvent> _levelPurchaseFilter;
    EcsFilter<PurchaseUpgrade1Event> _upgrade1PurchaseFilter;
    EcsFilter<PurchaseUpgrade2Event> _upgrade2PurchaseFilter;
    EcsFilter<Money> _moneyFilter;
    readonly StaticData _staticData;

    public PurchaseSystem(StaticData staticData) {
        _staticData = staticData;
    }

    public void Run() {
        ProcessLevelPurchases();
        ProcessUpgrade1();
        ProcessUpgrade2();
    }

    void ProcessLevelPurchases() {
        foreach (var eventIdx in _levelPurchaseFilter) {
            ref var purchaseEvent = ref _levelPurchaseFilter.Get1(eventIdx);
            
            if (!TryGetValidBusinessEntity(purchaseEvent.businessEntity, out var businessEntity)) continue;
            if (!TryGetMoneyEntity(out int moneyEntityIndex)) continue;

            ref var level = ref businessEntity.Get<Level>();
            ref var nextLevelCost = ref businessEntity.Get<NextLevelCost>();

            if (TryPurchase(moneyEntityIndex, nextLevelCost.value)) {
                level.value++;
                businessEntity.Get<DirtyBusinessUI>(); // Level changed
                Debug.Log($"Level {level.value} purchased");
            }
        }
    }


    void ProcessUpgrade1() {
        foreach (var eventIdx in _upgrade1PurchaseFilter) {
            ref var purchaseEvent = ref _upgrade1PurchaseFilter.Get1(eventIdx);
            
            if (!TryGetValidBusinessEntity(purchaseEvent.businessEntity, out var businessEntity)) continue;
            if (!TryGetMoneyEntity(out int moneyEntityIndex)) continue;

            ref var upgrade = ref businessEntity.Get<Upgrade1>();
            ref var businessData = ref businessEntity.Get<BusinessPresetIndex>();
            var preset = GetBusinessPreset(businessData.presetIndex);
            
            if (ProcessUpgradeLogic(moneyEntityIndex, ref upgrade.bought, preset.upgrade1Cost)) {
                businessEntity.Get<DirtyUpgradesUI>();
                businessEntity.Get<DirtyBusinessUI>(); // Income also changed
            }
        }
    }

    void ProcessUpgrade2() {
        foreach (var eventIdx in _upgrade2PurchaseFilter) {
            ref var purchaseEvent = ref _upgrade2PurchaseFilter.Get1(eventIdx);
            
            if (!TryGetValidBusinessEntity(purchaseEvent.businessEntity, out var businessEntity)) continue;
            if (!TryGetMoneyEntity(out int moneyEntityIndex)) continue;

            ref var upgrade = ref businessEntity.Get<Upgrade2>();
            ref var businessData = ref businessEntity.Get<BusinessPresetIndex>();
            var preset = GetBusinessPreset(businessData.presetIndex);
            
            if (ProcessUpgradeLogic(moneyEntityIndex, ref upgrade.bought, preset.upgrade2Cost)) {
                businessEntity.Get<DirtyUpgradesUI>();
                businessEntity.Get<DirtyBusinessUI>(); 
            }
        }
    }

    // Common logic for processing upgrade purchase
    bool ProcessUpgradeLogic(int moneyEntityIndex, ref bool upgradeBought, float cost) {
        if (upgradeBought) {
            Debug.LogWarning($"Улучшение уже куплено!");
            return false;
        }
        
        if (TryPurchase(moneyEntityIndex, cost)) {
            upgradeBought = true;
            Debug.Log($"Куплено улучшение");
            return true;
        }
        
        return false;
    }

    // Helper methods to avoid duplication

    bool TryGetValidBusinessEntity(EcsEntity businessEntity, out EcsEntity validEntity) {
        validEntity = businessEntity;
        
        if (!businessEntity.IsAlive()) {
            Debug.LogWarning("Попытка купить в несуществующем бизнесе!");
            return false;
        }

        return true;
    }

    bool TryGetMoneyEntity(out int moneyEntityIndex) {
        if (_moneyFilter.GetEntitiesCount() == 0) {
            Debug.LogError("Не найдена сущность с деньгами!");
            moneyEntityIndex = -1;
            return false;
        }

        moneyEntityIndex = 0;
        return true;
    }

    bool TryPurchase(int moneyEntityIndex, float cost) {
        if (cost < 0) {
            Debug.LogError($"Некорректная стоимость: {cost}");
            return false;
        }

        ref var money = ref _moneyFilter.Get1(moneyEntityIndex);

        if (money.value >= cost) {
            money.value -= cost;
            
            // Помечаем что деньги изменились
            var moneyEntity = _moneyFilter.GetEntity(moneyEntityIndex);
            if (!moneyEntity.Has<DirtyMoneyUI>()) {
                moneyEntity.Get<DirtyMoneyUI>();
            }
            
            return true;
        }

        Debug.Log($"Недостаточно средств для покупки! Нужно: {cost:F0}$, есть: {money.value:F0}$");
        return false;
    }

    BusinessPreset GetBusinessPreset(int presetIndex) {
        if (presetIndex < 0 || presetIndex >= _staticData.businesses.Length) {
            Debug.LogError($"Некорректный индекс бизнеса: {presetIndex}");
            return _staticData.businesses[0]; // Fallback
        }

        return _staticData.businesses[presetIndex];
    }
} 