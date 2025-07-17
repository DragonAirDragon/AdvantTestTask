using Leopotam.Ecs;

/// <summary>
/// Система обновления прогресса в единицах (0.0 - 1.0).
/// Формула: прошедшее_время / общее_время
/// </summary>
sealed class ProgressSystem : IEcsRunSystem {
    EcsFilter<Level, IncomeProgress> _filter;

    public void Run() {
        foreach (var i in _filter) {
            ref var level = ref _filter.Get1(i);
            ref var progress = ref _filter.Get2(i);

            // Прогресс только для купленных бизнесов (level > 0)
            if (level.value > 0) {
                // Прогресс в процентах (0.0 - 1.0)
                float progressValue = progress.elapsed / progress.delay;
                progressValue = UnityEngine.Mathf.Clamp01(progressValue);
                
                // Здесь можно отправить событие или обновить UI напрямую
                // В данном случае UI система сама будет читать эти данные
            }
        }
    }
} 