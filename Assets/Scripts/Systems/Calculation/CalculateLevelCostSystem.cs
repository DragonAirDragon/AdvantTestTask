using Leopotam.Ecs;

/// <summary>
/// System for calculating the cost of the next level.
/// Formula: (current_level + 1) * base_cost
/// Runs when level changes.
/// </summary>
sealed class CalculateLevelCostSystem : IEcsRunSystem {
    readonly EcsFilter<Level, NextLevelCost, BusinessPresetIndex> _filter;
    readonly StaticData _staticData;

    public CalculateLevelCostSystem(StaticData staticData) {
        _staticData = staticData;
    }

    public void Run() {
        foreach (var i in _filter) {
            ref var level = ref _filter.Get1(i);
            ref var nextLevelCost = ref _filter.Get2(i);
            ref var businessPresetIndex = ref _filter.Get3(i);

            var preset = _staticData.businesses[businessPresetIndex.presetIndex];
            nextLevelCost.value = (level.value + 1) * preset.baseCost;
        }
    }
} 