using Leopotam.Ecs;

/// <summary>
/// Система обновления UI для всех бизнесов.
/// Обновляет: уровень, доход, прогресс, стоимость улучшений, состояние кнопок.
/// </summary>
sealed class UISystem : IEcsRunSystem {
    EcsFilter<Level, IncomeProgress, NextLevelCost, BusinessData, Upgrade1, Upgrade2> _filter;
    EcsFilter<Money> _moneyFilter;
    readonly GameStaticData _staticData;
    readonly SceneData _sceneData;

    public UISystem(GameStaticData staticData, SceneData sceneData) {
        _staticData = staticData;
        _sceneData = sceneData;
    }

    public void Run() {
        float currentMoney = _moneyFilter.Get1(0).value;
        
        // Обновляем отображение баланса
        if (_sceneData.moneyText != null) {
            _sceneData.moneyText.text = $"Баланс: {currentMoney:F0}$";
        }

        foreach (var i in _filter) {
            ref var level = ref _filter.Get1(i);
            ref var progress = ref _filter.Get2(i);
            ref var nextLevelCost = ref _filter.Get3(i);
            ref var businessData = ref _filter.Get4(i);
            ref var upgrade1 = ref _filter.Get5(i);
            ref var upgrade2 = ref _filter.Get6(i);
            // Получаем ViewRef отдельным запросом
            var entity = _filter.GetEntity(i);
            ref var viewRef = ref entity.Get<ViewRef>();

            var preset = _staticData.businesses[businessData.presetIndex];
            var view = viewRef.view;

            // Обновляем уровень
            view.UpdateBusinessLevel(level.value);

            // Обновляем прогресс (только для купленных бизнесов)
            if (level.value > 0) {
                float progressValue = progress.elapsed / progress.delay;
                view.UpdateIncomeProgress(UnityEngine.Mathf.Clamp01(progressValue));
            } else {
                view.UpdateIncomeProgress(0f);
            }

            // Рассчитываем и обновляем доход
            float income = 0f;
            if (level.value > 0) {
                income = level.value * preset.baseIncome;
                float multiplier = 1f;
                if (upgrade1.bought) multiplier += preset.upgrade1IncomeMul - 1f;
                if (upgrade2.bought) multiplier += preset.upgrade2IncomeMul - 1f;
                income *= multiplier;
            }
            view.UpdateBusinessIncome(income);

            // Обновляем стоимость следующего уровня
            view.UpdateNextLevelCost(nextLevelCost.value);

            // Обновляем улучшения
            view.UpdateUpgrade1(preset.upgrade1Title, preset.upgrade1Cost, preset.upgrade1IncomeMul, upgrade1.bought);
            view.UpdateUpgrade2(preset.upgrade2Title, preset.upgrade2Cost, preset.upgrade2IncomeMul, upgrade2.bought);

            // TODO: Здесь нужно обновить интерактивность кнопок на основе доступных денег
            // Это требует модификации BusinessView для добавления методов управления кнопками
        }
    }
} 