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
            
            // Update UI
            if (_sceneData.moneyText != null) {
                _sceneData.moneyText.text = $"Balance: {money.value:F0}$";
            }
        }
    }
}