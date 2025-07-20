using System;
using UnityEngine;

/// <summary>
/// Static data for the game.(And Default data if no save)
/// Contains only gameplay data, not localized text.
/// </summary>
[CreateAssetMenu(menuName = "StaticData")]
public class StaticData : ScriptableObject {
    public BusinessPreset[] businesses;
    public int startMoney;
}

[Serializable]
public struct BusinessPreset {
    [Header("Identifier")]
    public string id;          

    [Header("Game Parameters")]
    public int startLevel;
    public float delay;       
    public float baseCost;    
    public float baseIncome;  
    
    [Header("Upgrade 1")]
    public float upgrade1Cost;        
    public float upgrade1IncomeMul;   

    [Header("Upgrade 2")]
    public float upgrade2Cost;        
    public float upgrade2IncomeMul;   
}