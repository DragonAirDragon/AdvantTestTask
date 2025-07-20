using Leopotam.Ecs;
using UnityEngine;

/// <summary>
/// Simple system for saving the game.
/// Saving only happens when the game is exited.
/// </summary>
public class SaveSystem : IEcsRunSystem {
    EcsFilter<Money> _moneyFilter;
    EcsFilter<BusinessId, Level, Upgrade1, Upgrade2, IncomeProgress> _businessFilter;
    EcsFilter<SaveGameEvent> _saveEventFilter;
    
    public void Run() {
        // Save only when there is a SaveGameEvent
        foreach (var i in _saveEventFilter) {
            SaveGame();
        }
    }
    
    /// <summary>
    /// Saves the game when exiting.
    /// </summary>
    void SaveGame() {
        try {
            var saveData = new GameSaveData();
            
            foreach (var i in _moneyFilter) {
                saveData.playerMoney = _moneyFilter.Get1(i).value;
            }
            
            foreach (var i in _businessFilter) {
                var businessData = new BusinessSaveData {
                    businessId = _businessFilter.Get1(i).value,
                    level = _businessFilter.Get2(i).value,
                    upgrade1Bought = _businessFilter.Get3(i).bought,
                    upgrade2Bought = _businessFilter.Get4(i).bought,
                    elapsedTime = _businessFilter.Get5(i).elapsed
                };
                saveData.businesses.Add(businessData);
            }
            
            string json = JsonUtility.ToJson(saveData, true);
            PlayerPrefs.SetString("GameSave", json);
            PlayerPrefs.Save();
            Debug.Log("Game saved!");
        }
        catch (System.Exception e) {
            Debug.LogError($"Error saving game: {e.Message}");
        }
    }
}