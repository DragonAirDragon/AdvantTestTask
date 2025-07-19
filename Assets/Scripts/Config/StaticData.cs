using System;
using UnityEngine;
/// <summary>
/// Static data for the game.(And Default data if no save)
/// </summary>
[CreateAssetMenu(menuName = "StaticData")]
public class StaticData : ScriptableObject {
    public BusinessPreset[] businesses;
    public int startMoney;
}

[Serializable]
public struct BusinessPreset {
    public string  id;          
    public string   displayName; 

    public int startLevel;

    public float    delay;       
    public float  baseCost;    
    public float  baseIncome;  
    
    // Upgrade 1
    public string   upgrade1Title;       
    public float  upgrade1Cost;        
    public float    upgrade1IncomeMul;   

    // Upgrade 2
    public string   upgrade2Title;       
    public float  upgrade2Cost;        
    public float    upgrade2IncomeMul;   
}