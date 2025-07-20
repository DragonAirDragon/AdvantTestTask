using Leopotam.Ecs;
using UnityEngine;

sealed class SpawnBusinessesSystem : IEcsInitSystem {
    readonly StaticData _staticData;
    readonly SceneData _scene;
    readonly LoadService _loadService; 
    readonly BusinessNamesData _localizationData;
    EcsWorld _world;

    public SpawnBusinessesSystem(StaticData staticData, SceneData scene, LoadService loadService, BusinessNamesData localizationData) {
        _staticData = staticData;
        _scene = scene;
        _loadService = loadService;
        _localizationData = localizationData;
    }

    public void Init () {
        // Load data through the service
        var saveData = _loadService.LoadGame();
        
        // Create entity with player's money
        var moneyEntity = _world.NewEntity();
        ref var money = ref moneyEntity.Get<Money>();
        money.value = saveData?.playerMoney ?? _staticData.startMoney;
        
        // Create businesses
        for (int i = 0; i < _staticData.businesses.Length; i++) {
            ref readonly var preset = ref _staticData.businesses[i];
            
            EcsEntity ent = _world.NewEntity();
            
            // Main components
            ref var businessId = ref ent.Get<BusinessId>();
            ref var businessPresetIndex = ref ent.Get<BusinessPresetIndex>();
            ref var level = ref ent.Get<Level>();
            ref var progress = ref ent.Get<IncomeProgress>();
            ref var nextLevelCost = ref ent.Get<NextLevelCost>();
            ref var upgrade1 = ref ent.Get<Upgrade1>();
            ref var upgrade2 = ref ent.Get<Upgrade2>();
            ref var calculatedIncome = ref ent.Get<CalculatedIncome>();
            ref var viewRef = ref ent.Get<ViewRef>();

            // Initialize base data
            businessId.value = preset.id;
            businessPresetIndex.presetIndex = i;
            progress.delay = preset.delay;

            // Load saved data for the business
            BusinessSaveData businessSave = null;
            if (saveData != null) {
                string presetId = preset.id;
                businessSave = saveData.businesses.Find(b => b.businessId == presetId);
            }

            // Apply saved data FIRST
            level.value = businessSave?.level ?? preset.startLevel;
            upgrade1.bought = businessSave?.upgrade1Bought ?? false;
            upgrade2.bought = businessSave?.upgrade2Bought ?? false;
            progress.elapsed = businessSave?.elapsedTime ?? 0;

            // Calculate dependent values AFTER loading data
            nextLevelCost.value = (level.value + 1) * preset.baseCost;

            // Calculate income for correct display
            float income = level.value > 0 ? level.value * preset.baseIncome : 0f;
            float multiplier = 1f;
            if (upgrade1.bought) multiplier += preset.upgrade1IncomeMul - 1f;
            if (upgrade2.bought) multiplier += preset.upgrade2IncomeMul - 1f;
            calculatedIncome.value = income * multiplier;

            // Create and setup View
            BusinessView view = Object.Instantiate(_scene.businessViewPrefab, _scene.businessListRoot);
            viewRef.view = view;

            // Get localized data
            var localization = _localizationData.GetBusinessNames(preset.id);

            view.UpdateBusinessName(localization.displayName);
            view.UpdateBusinessLevel(level.value);
            view.UpdateBusinessIncome(calculatedIncome.value);
            view.UpdateNextLevelCost(nextLevelCost.value);

            view.UpdateUpgrade1(localization.upgrade1Title, preset.upgrade1Cost, preset.upgrade1IncomeMul, upgrade1.bought);
            view.UpdateUpgrade2(localization.upgrade2Title, preset.upgrade2Cost, preset.upgrade2IncomeMul, upgrade2.bought);
            
            // Setup controller for handling UI events
            var controller = view.gameObject.AddComponent<BusinessController>();
            controller.Init(_world, ent);
            
            // Mark for initial UI display
            ent.Get<DirtyBusinessUI>();
            ent.Get<DirtyUpgradesUI>();
        }
        
        // Mark money for initial display
        moneyEntity.Get<DirtyMoneyUI>();
    }
}
