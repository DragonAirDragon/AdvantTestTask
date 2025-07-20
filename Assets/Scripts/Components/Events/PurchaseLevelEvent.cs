using Leopotam.Ecs;

/// <summary>
/// Event for business level purchase.
/// Added to entity when clicking the level upgrade button.
/// </summary>
public struct PurchaseLevelEvent {
    public EcsEntity businessEntity;
} 