using UnityEngine;

/// <summary>
/// Service for loading game data.
/// Not an ECS system - just a service class.
/// </summary>
public class LoadService {
    
    /// <summary>
    /// Loads saved game data.
    /// </summary>
    public GameSaveData LoadGame() {
        if (!PlayerPrefs.HasKey("GameSave")) {
            Debug.Log("Save not found, creating new game");
            return null;
        }
        
        try {
            string json = PlayerPrefs.GetString("GameSave");
            var saveData = JsonUtility.FromJson<GameSaveData>(json);
            
            Debug.Log($"Game loaded! Money: {saveData.playerMoney}, businesses: {saveData.businesses.Count}");
            return saveData;
        }
        catch (System.Exception e) {
            Debug.LogError($"Error loading save: {e.Message}");
            return null;
        }
    }
} 