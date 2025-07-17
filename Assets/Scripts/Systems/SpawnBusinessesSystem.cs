using Leopotam.Ecs;
using UnityEngine;

sealed class SpawnBusinessesSystem : IEcsInitSystem {
    readonly GameStaticData _staticData;
    readonly SceneData _scene;
    EcsWorld _world;

    public SpawnBusinessesSystem(GameStaticData staticData, SceneData scene) {
        _staticData = staticData;
        _scene = scene;
    }

    public void Init () {
        // Создаем entity с деньгами игрока
        var moneyEntity = _world.NewEntity();
        ref var money = ref moneyEntity.Get<Money>();
        money.value = 100f; // Начальные деньги

        // Создаем бизнесы
        for (int i = 0; i < _staticData.businesses.Length; i++) {
            ref readonly var preset = ref _staticData.businesses[i];
            
            EcsEntity ent = _world.NewEntity();
            
            // Основные компоненты
            ref var businessId = ref ent.Get<BusinessId>();
            ref var level = ref ent.Get<Level>();
            ref var progress = ref ent.Get<IncomeProgress>();
            ref var nextLevelCost = ref ent.Get<NextLevelCost>();
            
            // Новые компоненты
            ref var businessData = ref ent.Get<BusinessData>();
            ref var upgrade1 = ref ent.Get<Upgrade1>();
            ref var upgrade2 = ref ent.Get<Upgrade2>();
            ref var viewRef = ref ent.Get<ViewRef>();

            // Инициализация данных
            businessId.value = preset.id;
            level.value = 0;
            progress.delay = preset.delay;
            progress.elapsed = 0f;
            nextLevelCost.value = (level.value + 1) * preset.baseCost;
            businessData.presetIndex = i;
            upgrade1.bought = false;
            upgrade2.bought = false;

            // Создание и настройка View
            BusinessView view = Object.Instantiate(_scene.businessViewPrefab, _scene.businessListRoot);
            viewRef.view = view;
           
            view.Init();
            view.UpdateBusinessName(preset.displayName);
            view.UpdateBusinessLevel(level.value);
            view.UpdateBusinessIncome(0);
            view.UpdateNextLevelCost(nextLevelCost.value);
            view.UpdateUpgrade1(preset.upgrade1Title, preset.upgrade1Cost, preset.upgrade1IncomeMul, false);
            view.UpdateUpgrade2(preset.upgrade2Title, preset.upgrade2Cost, preset.upgrade2IncomeMul, false);
            
            // Настройка контроллера для обработки событий UI
            var controller = view.gameObject.AddComponent<BusinessController>();
            controller.Init(_world, preset.id);
        }
    }
}
