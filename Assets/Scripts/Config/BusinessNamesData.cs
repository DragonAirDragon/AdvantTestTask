using System;
using UnityEngine;

/// <summary>
/// ScriptableObject for storing business names and upgrades.
/// </summary>
[CreateAssetMenu(menuName = "BusinessNamesData", fileName = "BusinessNamesData")]
public class BusinessNamesData : ScriptableObject {
    [Header("Businesses")]
    public BusinessNames[] businesses;
    
    /// <summary>
    /// Finds business names by Preset ID   
    /// </summary>
    public BusinessNames GetBusinessNames(string businessId) {
        foreach (var business in businesses) {
            if (business.businessPresetId == businessId) {
                return business;
            }
        }
        
        Debug.LogWarning($"Names not found for business ID: {businessId}");
        return businesses.Length > 0 ? businesses[0] : new BusinessNames();
    }
}

[Serializable]
public struct BusinessNames {
    [Header("Business Preset ID")]
    public string businessPresetId;
    
    [Header("Display Name")]
    public string displayName;
    
    [Header("Upgrade Names")]
    public string upgrade1Title;
    public string upgrade2Title;
} 