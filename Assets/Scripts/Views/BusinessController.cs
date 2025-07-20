using Leopotam.Ecs;
using UnityEngine;

/// <summary>
/// Controller for connecting BusinessView with ECS system.
/// Handles UI events and creates corresponding ECS events.
/// </summary>
public class BusinessController : MonoBehaviour {
    private EcsWorld _world;
    private EcsEntity _businessEntity;
    
    public void Init(EcsWorld world, EcsEntity businessEntity) {
        _world = world;
        _businessEntity = businessEntity;
        
        // Подключаем обработчики кнопок
        var view = GetComponent<BusinessView>();
        view.SetupCallbacks(OnBuyLevel, OnBuyUpgrade1, OnBuyUpgrade2);
    }
    
    private void OnBuyLevel() {
        if (_world == null) return;
        
        var entity = _world.NewEntity();
        ref var purchaseEvent = ref entity.Get<PurchaseLevelEvent>();
        purchaseEvent.businessEntity = _businessEntity;
        
    }
    
    private void OnBuyUpgrade1() {
        if (_world == null) return;
        
        var entity = _world.NewEntity();
        ref var purchaseEvent = ref entity.Get<PurchaseUpgrade1Event>();
        purchaseEvent.businessEntity = _businessEntity;
        
    }
    
    private void OnBuyUpgrade2() {
        if (_world == null) return;
        
        var entity = _world.NewEntity();
        ref var purchaseEvent = ref entity.Get<PurchaseUpgrade2Event>();
        purchaseEvent.businessEntity = _businessEntity;
        
    }
} 