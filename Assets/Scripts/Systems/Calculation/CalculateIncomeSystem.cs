using Leopotam.Ecs;

/// <summary>
/// System for calculating business income with level and upgrades.
/// Caches result in CalculatedIncome component.
/// </summary>
public class CalculateIncomeSystem : IEcsRunSystem {
    private StaticData _staticData;
    
    // Filter for businesses with changed income parameters
    private EcsFilter<Level, BusinessPresetIndex, Upgrade1, Upgrade2, CalculatedIncome> _filter;
    
    public CalculateIncomeSystem(StaticData staticData) {
        _staticData = staticData;
    }
    
    public void Run() {
        foreach (var i in _filter) {
            ref var level = ref _filter.Get1(i);
            ref var businessPresetIndex = ref _filter.Get2(i);
            ref var upgrade1 = ref _filter.Get3(i);
            ref var upgrade2 = ref _filter.Get4(i);
            ref var calculatedIncome = ref _filter.Get5(i);
            
            // Calculate income only if there is a level
            if (level.value <= 0) {
                calculatedIncome.value = 0f;
                continue;
            }
            
            var preset = _staticData.businesses[businessPresetIndex.presetIndex];
            float income = level.value * preset.baseIncome;
            
            // Apply upgrade multipliers
            float multiplier = 1f;
            if (upgrade1.bought) multiplier += preset.upgrade1IncomeMul - 1f;
            if (upgrade2.bought) multiplier += preset.upgrade2IncomeMul - 1f;
            
            calculatedIncome.value = income * multiplier;
        }
    }
} 