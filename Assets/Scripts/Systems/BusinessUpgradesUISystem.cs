using Leopotam.Ecs;

/// <summary>
/// Reactive system for updating business upgrades UI.
/// Updates information about upgrades and button states.
/// Works only with businesses marked with DirtyUpgradesUI.
/// </summary>
sealed class BusinessUpgradesUISystem : IEcsRunSystem
{
    readonly StaticData _staticData;
    EcsFilter<BusinessPresetIndex, Upgrade1, Upgrade2, ViewRef, DirtyUpgradesUI> _dirtyUpgradesFilter;
    
    public BusinessUpgradesUISystem(StaticData staticData)
    {
        _staticData = staticData;
    }
    
    public void Run()
    {
        foreach (var i in _dirtyUpgradesFilter) {
            ref var businessPresetIndex = ref _dirtyUpgradesFilter.Get1(i);
            ref var upgrade1 = ref _dirtyUpgradesFilter.Get2(i);
            ref var upgrade2 = ref _dirtyUpgradesFilter.Get3(i);
            ref var viewRef = ref _dirtyUpgradesFilter.Get4(i);

            var preset = _staticData.businesses[businessPresetIndex.presetIndex];


            viewRef.view.UpdateUpgrade1(preset.upgrade1Title, preset.upgrade1Cost, preset.upgrade1IncomeMul, upgrade1.bought);
            viewRef.view.UpdateUpgrade2(preset.upgrade2Title, preset.upgrade2Cost, preset.upgrade2IncomeMul, upgrade2.bought);
        }
    }
}