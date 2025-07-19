using System;
using System.Collections.Generic;

/// <summary>
/// Model for saving game progress.
/// Serialized to JSON when exiting the game.
/// </summary>
[Serializable]
public class GameSaveData {
    public float playerMoney;
    public List<BusinessSaveData> businesses = new List<BusinessSaveData>();
}

[Serializable]
public class BusinessSaveData {
    public string businessId;
    public int level;
    public bool upgrade1Bought;
    public bool upgrade2Bought;
    public float elapsedTime;
}