using Leopotam.Ecs;
using UnityEngine;

/// <summary>
/// System for calculating and adding income.
/// Uses pre-calculated income from CalculatedIncome component.
/// Logic is not executed if level == 0.
/// Adds dirty markers for updating UI.
/// </summary>
sealed class IncomeSystem : IEcsRunSystem {
    EcsFilter<Level, IncomeProgress, BusinessPresetIndex, CalculatedIncome> _businessesFilter;
    EcsFilter<Money> _moneyFilter;
    readonly StaticData _staticData;

    public IncomeSystem(StaticData staticData) {
        _staticData = staticData;
    }

    public void Run() {
        // Get entity with money
        ref var money = ref _moneyFilter.Get1(0);
        var moneyEntity = _moneyFilter.GetEntity(0);

        foreach (var i in _businessesFilter) {
            ref var level = ref _businessesFilter.Get1(i);
            ref var incomeProgress = ref _businessesFilter.Get2(i);
            ref var businessPresetIndex = ref _businessesFilter.Get3(i);
            ref var calculatedIncome = ref _businessesFilter.Get4(i);
        
            // Skip if level is 0 (business not purchased)
            if (level.value == 0) continue;
        
            // Update progress
            incomeProgress.elapsed += Time.deltaTime;
        
            // If income time has come
            if (incomeProgress.elapsed >= incomeProgress.delay) {
                // Reset timer
                incomeProgress.elapsed = 0f;
                
                // Add pre-calculated income
                money.value += calculatedIncome.value;
                
                // Mark that money has changed - need to update UI
                if (!moneyEntity.Has<DirtyMoneyUI>()) {
                    moneyEntity.Get<DirtyMoneyUI>();
                }
                
                var preset = _staticData.businesses[businessPresetIndex.presetIndex];
                Debug.Log($"Business {preset.displayName} earned: {calculatedIncome.value}$");
            }
        }
    }
} 