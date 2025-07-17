using Leopotam.Ecs;
using UnityEngine;

/// <summary>
/// Система расчета и начисления дохода.
/// Формула: LVL * baseIncome * (1 + upgrade1.incomeMul + upgrade2.incomeMul)
/// Логика не выполняется если level == 0
/// </summary>
sealed class IncomeSystem : IEcsRunSystem {
    EcsFilter<Level, IncomeProgress, BusinessData, Upgrade1, Upgrade2> _businessesFilter;
    EcsFilter<Money> _moneyFilter;
    readonly GameStaticData _staticData;

    public IncomeSystem(GameStaticData staticData) {
        _staticData = staticData;
    }

    public void Run() {
        // Получаем сущность с деньгами
        ref var money = ref _moneyFilter.Get1(0);
        
        foreach (var i in _businessesFilter) {
            ref var level = ref _businessesFilter.Get1(i);
            ref var progress = ref _businessesFilter.Get2(i);
            ref var businessData = ref _businessesFilter.Get3(i);
            ref var upgrade1 = ref _businessesFilter.Get4(i);
            ref var upgrade2 = ref _businessesFilter.Get5(i);

            // Пропускаем если уровень 0 (бизнес не куплен)
            if (level.value == 0) continue;

            // Обновляем прогресс
            progress.elapsed += Time.deltaTime;

            // Если время дохода пришло
            if (progress.elapsed >= progress.delay) {
                // Сброс таймера
                progress.elapsed = 0f;
                
                // Расчет дохода по формуле
                var preset = _staticData.businesses[businessData.presetIndex];
                float income = level.value * preset.baseIncome;
                
                // Применяем мультипликаторы улучшений
                float multiplier = 1f;
                if (upgrade1.bought) multiplier += preset.upgrade1IncomeMul - 1f;
                if (upgrade2.bought) multiplier += preset.upgrade2IncomeMul - 1f;
                
                income *= multiplier;
                
                // Начисляем доход
                money.value += income;
                
                Debug.Log($"Бизнес {preset.displayName} заработал: {income}$");
            }
        }
    }
} 