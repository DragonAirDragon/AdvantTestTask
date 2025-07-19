using Leopotam.Ecs;

/// <summary>
/// System for updating income progress bars.
/// Works every frame, but only updates the slider - the lightest UI element.
/// All other elements are updated via Reactive UI.
/// </summary>
sealed class IncomeProgressUISystem : IEcsRunSystem {
    EcsFilter<Level, IncomeProgress, ViewRef> _businessFilter;

    public void Run() {
        foreach (var i in _businessFilter) {
            ref var level = ref _businessFilter.Get1(i);
            ref var incomeProgress = ref _businessFilter.Get2(i);
            ref var viewRef = ref _businessFilter.Get3(i);
            var view = viewRef.view;

            // Обновляем только прогресс (легковесная операция)
            if (level.value > 0) {
                float progressValue = incomeProgress.elapsed / incomeProgress.delay;
                view.UpdateIncomeProgress(UnityEngine.Mathf.Clamp01(progressValue));
            } else {
                view.UpdateIncomeProgress(0f);
            }
        }
    }
} 