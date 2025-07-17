using Leopotam.Ecs;
using UnityEngine;

/// <summary>
/// Контроллер для связи BusinessView с ECS системой.
/// Обрабатывает события UI и создает соответствующие ECS события.
/// </summary>
public class BusinessController : MonoBehaviour {
    private EcsWorld _world;
    private string _businessId;
    
    public void Init(EcsWorld world, string businessId) {
        _world = world;
        _businessId = businessId;
        
        // Подключаем обработчики кнопок
        var view = GetComponent<BusinessView>();
        view.SetupCallbacks(OnBuyLevel, OnBuyUpgrade1, OnBuyUpgrade2);
    }
    
    private void OnBuyLevel() {
        if (_world == null) return;
        
        var entity = _world.NewEntity();
        ref var purchaseEvent = ref entity.Get<PurchaseLevelEvent>();
        purchaseEvent.businessId = _businessId;
        
        Debug.Log($"Создано событие покупки уровня для {_businessId}");
    }
    
    private void OnBuyUpgrade1() {
        if (_world == null) return;
        
        var entity = _world.NewEntity();
        ref var purchaseEvent = ref entity.Get<PurchaseUpgrade1Event>();
        purchaseEvent.businessId = _businessId;
        
        Debug.Log($"Создано событие покупки улучшения 1 для {_businessId}");
    }
    
    private void OnBuyUpgrade2() {
        if (_world == null) return;
        
        var entity = _world.NewEntity();
        ref var purchaseEvent = ref entity.Get<PurchaseUpgrade2Event>();
        purchaseEvent.businessId = _businessId;
        
        Debug.Log($"Создано событие покупки улучшения 2 для {_businessId}");
    }
} 