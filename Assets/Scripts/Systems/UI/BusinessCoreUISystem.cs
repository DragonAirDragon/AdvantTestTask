using Leopotam.Ecs;

/// <summary>
/// Reactive system for updating business core UI data.
/// Updates: level, income, next level cost.
/// Works only with businesses marked with DirtyBusinessUI.
/// </summary>
sealed class BusinessCoreUISystem : IEcsRunSystem {
    EcsFilter<Level, NextLevelCost, ViewRef, CalculatedIncome, DirtyBusinessUI> _dirtyBusinessFilter;

    public void Run() {
        foreach (var i in _dirtyBusinessFilter) {
            ref var level = ref _dirtyBusinessFilter.Get1(i);
            ref var nextLevelCost = ref _dirtyBusinessFilter.Get2(i);
            ref var viewRef = ref _dirtyBusinessFilter.Get3(i);
            ref var calculatedIncome = ref _dirtyBusinessFilter.Get4(i);
            var view = viewRef.view;

            // Update level
            view.UpdateBusinessLevel(level.value);
            
            // Use pre-calculated income
            view.UpdateBusinessIncome(calculatedIncome.value);
            
            // Update next level cost
            view.UpdateNextLevelCost(nextLevelCost.value);
        }
    }
} 