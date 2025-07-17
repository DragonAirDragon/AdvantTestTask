using System;
using UnityEngine;
/// <summary>
/// Дефолтные данные для игры (если нет сохранений)
/// </summary>
[CreateAssetMenu(menuName = "GameStaticData")]
public class GameStaticData : ScriptableObject {
    public BusinessPreset[] businesses;
}

[Serializable]
public struct BusinessPreset {
    public string  id;          // "lemonadeStand"
    public string   displayName; // "Лимонадная будка"




    public float    delay;       // 1.0 c
    public float  baseCost;    // 4
    public float  baseIncome;  // 1
    
    
    // Upgrade 1
    public string   upgrade1Title;       // "2× скорость"
    public float  upgrade1Cost;        // 500
    public float    upgrade1IncomeMul;   // 2 ×

    // Upgrade 2
    public string   upgrade2Title;       // "2× скорость"
    public float  upgrade2Cost;        // 500
    public float    upgrade2IncomeMul;   // 2 ×
}


// Доход = LVL * baseIncome * (1 + upgrade1.incomeMul + upgrade2.incomeMul)
// Цена уровня = (LVL + 1) * baseCost 

// Получается для бизнеса в динамическом режиме нужны такие данные:
// - LVL БИЗНЕСА
// - ЦЕНА УРОВНЯ БИЗНЕСА
// - ПРОГРЕСС ЗАРАБОТКА

// Нужно выделить компоненты для этих данных