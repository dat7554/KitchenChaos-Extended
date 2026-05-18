using System.Collections.Generic;
using UnityEngine;

public static class UpgradeManager
{
    private static HashSet<UpgradeSO> _activeUpgradeSOSet = new();

    public static void AddUpgrade(UpgradeSO upgradeSO)
    {
        _activeUpgradeSOSet.Add(upgradeSO);
    }

    public static bool HasUpgrade(UpgradeSO upgradeSO)
    {
        return _activeUpgradeSOSet.Contains(upgradeSO);
    }

    public static void ResetUpgrades()
    {
        _activeUpgradeSOSet.Clear();
    }

    public static void LogActiveUpgrade()
    {
        foreach (var upgradeSO in _activeUpgradeSOSet)
        {
            Debug.Log(upgradeSO.displayName);
        }
    }
}
