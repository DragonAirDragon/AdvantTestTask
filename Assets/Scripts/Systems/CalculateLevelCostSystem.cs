using Leopotam.Ecs;

/// <summary>
/// Система расчета стоимости следующего уровня.
/// Формула: (текущий_уровень + 1) * базовая_стоимость
/// Запускается при изменении уровня.
/// </summary>
sealed class CalculateLevelCostSystem : IEcsRunSystem {
    EcsFilter<Level, NextLevelCost, BusinessData> _filter;
    readonly GameStaticData _staticData;

    public CalculateLevelCostSystem(GameStaticData staticData) {
        _staticData = staticData;
    }

    public void Run() {
        foreach (var i in _filter) {
            ref var level = ref _filter.Get1(i);
            ref var nextLevelCost = ref _filter.Get2(i);
            ref var businessData = ref _filter.Get3(i);

            var preset = _staticData.businesses[businessData.presetIndex];
            nextLevelCost.value = (level.value + 1) * preset.baseCost;
        }
    }
} 