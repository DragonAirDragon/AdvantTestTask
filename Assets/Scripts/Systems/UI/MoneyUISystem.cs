using Leopotam.Ecs;

/// <summary>
/// Reactive system for updating money UI.
/// Updates only when there is a DirtyMoneyUI marker.
/// </summary>
sealed class MoneyUISystem : IEcsRunSystem
{
    EcsFilter<Money, DirtyMoneyUI> _dirtyMoneyFilter;
    readonly SceneData _sceneData;

    public MoneyUISystem(SceneData sceneData){
        _sceneData = sceneData;
    }
    
    public void Run()
    {
        foreach (var i in _dirtyMoneyFilter) {
            ref var money = ref _dirtyMoneyFilter.Get1(i);
            
            // Update UI with localized format
            if (_sceneData.moneyText != null) {
                string formattedMoney = string.Format("Баланс: {0}$", money.value.ToString("F0"));
                _sceneData.moneyText.text = formattedMoney;
            }
        }
    }
}