using Leopotam.Ecs;
using UnityEngine;

/// <summary>
/// Система обработки покупок (уровни и улучшения).
/// Обрабатывает события покупок и проверяет достаточность средств.
/// </summary>
sealed class PurchaseSystem : IEcsRunSystem {
    EcsFilter<PurchaseLevelEvent> _levelPurchaseFilter;
    EcsFilter<PurchaseUpgrade1Event> _upgrade1PurchaseFilter;
    EcsFilter<PurchaseUpgrade2Event> _upgrade2PurchaseFilter;
    EcsFilter<BusinessId, Level, NextLevelCost, Upgrade1, Upgrade2, BusinessData> _businessFilter;
    EcsFilter<Money> _moneyFilter;
    readonly GameStaticData _staticData;

    public PurchaseSystem(GameStaticData staticData) {
        _staticData = staticData;
    }

    public void Run() {
        ProcessLevelPurchases();
        ProcessUpgrade1Purchases();
        ProcessUpgrade2Purchases();
    }

    void ProcessLevelPurchases() {
        foreach (var eventIdx in _levelPurchaseFilter) {
            ref var purchaseEvent = ref _levelPurchaseFilter.Get1(eventIdx);
            ref var money = ref _moneyFilter.Get1(0);

            // Найти бизнес по ID
            foreach (var businessIdx in _businessFilter) {
                ref var businessId = ref _businessFilter.Get1(businessIdx);
                
                if (businessId.value != purchaseEvent.businessId) continue;

                ref var level = ref _businessFilter.Get2(businessIdx);
                ref var nextLevelCost = ref _businessFilter.Get3(businessIdx);

                // Проверить достаточность средств
                if (money.value >= nextLevelCost.value) {
                    // Списать деньги
                    money.value -= nextLevelCost.value;
                    
                    // Повысить уровень
                    level.value++;
                    
                    Debug.Log($"Куплен уровень {level.value} для бизнеса {businessId.value}");
                } else {
                    Debug.Log("Недостаточно средств для покупки уровня!");
                }
                break;
            }

            // Удалить событие
            _levelPurchaseFilter.GetEntity(eventIdx).Del<PurchaseLevelEvent>();
        }
    }

    void ProcessUpgrade1Purchases() {
        foreach (var eventIdx in _upgrade1PurchaseFilter) {
            ref var purchaseEvent = ref _upgrade1PurchaseFilter.Get1(eventIdx);
            ref var money = ref _moneyFilter.Get1(0);

            foreach (var businessIdx in _businessFilter) {
                ref var businessId = ref _businessFilter.Get1(businessIdx);
                
                if (businessId.value != purchaseEvent.businessId) continue;

                ref var upgrade1 = ref _businessFilter.Get4(businessIdx);
                ref var businessData = ref _businessFilter.Get6(businessIdx);

                if (upgrade1.bought) {
                    Debug.Log("Улучшение 1 уже куплено!");
                    break;
                }

                var preset = _staticData.businesses[businessData.presetIndex];

                if (money.value >= preset.upgrade1Cost) {
                    money.value -= preset.upgrade1Cost;
                    upgrade1.bought = true;
                    
                    Debug.Log($"Куплено улучшение 1 для бизнеса {businessId.value}");
                } else {
                    Debug.Log("Недостаточно средств для покупки улучшения 1!");
                }
                break;
            }

            _upgrade1PurchaseFilter.GetEntity(eventIdx).Del<PurchaseUpgrade1Event>();
        }
    }

    void ProcessUpgrade2Purchases() {
        foreach (var eventIdx in _upgrade2PurchaseFilter) {
            ref var purchaseEvent = ref _upgrade2PurchaseFilter.Get1(eventIdx);
            ref var money = ref _moneyFilter.Get1(0);

            foreach (var businessIdx in _businessFilter) {
                ref var businessId = ref _businessFilter.Get1(businessIdx);
                
                if (businessId.value != purchaseEvent.businessId) continue;

                ref var upgrade2 = ref _businessFilter.Get5(businessIdx);
                ref var businessData = ref _businessFilter.Get6(businessIdx);

                if (upgrade2.bought) {
                    Debug.Log("Улучшение 2 уже куплено!");
                    break;
                }

                var preset = _staticData.businesses[businessData.presetIndex];

                if (money.value >= preset.upgrade2Cost) {
                    money.value -= preset.upgrade2Cost;
                    upgrade2.bought = true;
                    
                    Debug.Log($"Куплено улучшение 2 для бизнеса {businessId.value}");
                } else {
                    Debug.Log("Недостаточно средств для покупки улучшения 2!");
                }
                break;
            }

            _upgrade2PurchaseFilter.GetEntity(eventIdx).Del<PurchaseUpgrade2Event>();
        }
    }
} 