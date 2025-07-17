
using Leopotam.Ecs;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{
    [SerializeField] private GameStaticData _staticData;
    [SerializeField] private SceneData _sceneData;    
    private EcsWorld _world;
    private EcsSystems _systems;

    private void Start()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);

        // Система инициализации (выполняется один раз)
        _systems.Add(new SpawnBusinessesSystem(_staticData, _sceneData));
        
        // Игровые системы (выполняются каждый кадр)
        _systems.Add(new IncomeSystem(_staticData));
        _systems.Add(new CalculateLevelCostSystem(_staticData));
        _systems.Add(new ProgressSystem());
        _systems.Add(new PurchaseSystem(_staticData));
        _systems.Add(new UISystem(_staticData, _sceneData));

        _systems.Init();    
    }


    private void Update()
    {
        _systems?.Run();
    }

    private void OnDestroy()
    {
        _systems?.Destroy();
        _systems = null;
        _world?.Destroy();
        _world = null;
    }
}
