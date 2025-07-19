
using Leopotam.Ecs;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{
    [SerializeField] private StaticData _staticData;
    [SerializeField] private SceneData _sceneData;    
    private EcsWorld _world;
    private EcsSystems _systems;

    private void Start()
    {
        ClearSave();
        try
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            
            // Create load service
            var loadService = new LoadService();

            // Game systems
            _systems.Add(new SpawnBusinessesSystem(_staticData, _sceneData, loadService))
                    .Add(new IncomeSystem(_staticData))           
                    .Add(new PurchaseSystem(_staticData))         
                    .Add(new CalculateIncomeSystem(_staticData))  
                    .Add(new CalculateLevelCostSystem(_staticData)) 
                    
                    // UI systems (executed after all calculations)
                    .Add(new MoneyUISystem(_sceneData))
                    .Add(new BusinessCoreUISystem())
                    .Add(new BusinessUpgradesUISystem(_staticData))
                    .Add(new IncomeProgressUISystem())
                    
                    // Save system
                    .Add(new SaveSystem())

                    .OneFrame<PurchaseLevelEvent>()
                    .OneFrame<PurchaseUpgrade1Event>()
                    .OneFrame<PurchaseUpgrade2Event>()
                    .OneFrame<SaveGameEvent>() 
                    .OneFrame<DirtyMoneyUI>()      
                    .OneFrame<DirtyBusinessUI>()
                    .OneFrame<DirtyUpgradesUI>();        

            _systems.Init();
            Debug.Log("ECS Systems initialized successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error initializing ECS: {e.Message}\n{e.StackTrace}");
        }
    }

    private void Update()
    {
        _systems?.Run();
    }

    // Methods for saving when exiting
    void OnApplicationPause(bool pauseStatus) {
        if (pauseStatus) {
            TriggerSave();
        }
    }

    void OnApplicationFocus(bool hasFocus) {
        if (!hasFocus) {
            TriggerSave();
        }
    }

    private void OnDestroy()
    {
        TriggerSave();

        _systems?.Destroy();
        _systems = null;
        _world?.Destroy();
        _world = null;
    }

    void ClearSave() {
        PlayerPrefs.DeleteKey("GameSave");
        PlayerPrefs.Save();
        Debug.Log("Save cleared!");
    }


    void TriggerSave() {
        if (_world != null) {
            var saveEntity = _world.NewEntity();
            saveEntity.Get<SaveGameEvent>();
            
            // Force one cycle for saving processing
            _systems?.Run();
        }
    }
}
