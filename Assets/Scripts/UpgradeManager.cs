using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UpgradeManager
{
    public enum UpgradeType 
    { 
        WarmingLamps, 
        SharpKnives, 
        HotStoves, 
        BiggerBoard, 
        GenerousTips 
    }
    
    private static HashSet<UpgradeSO> _activeUpgradeSOSet = new();

    public static void AddUpgrade(UpgradeSO upgradeSO) => _activeUpgradeSOSet.Add(upgradeSO);
    public static bool HasUpgrade(UpgradeSO upgradeSO) => _activeUpgradeSOSet.Contains(upgradeSO);
    public static void ResetUpgrades() => _activeUpgradeSOSet.Clear();
    public static UpgradeSO[] GetActiveUpgrades() => _activeUpgradeSOSet.ToArray();

    public static void LogActiveUpgrade()
    {
        foreach (var upgradeSO in _activeUpgradeSOSet)
        {
            Debug.Log(upgradeSO.displayName);
        }
    }
    
    // called by Order
    public static float GetOrderTimeMultiplier()
    {
        float multiplier = 1f;
        foreach (var upgradeSO in _activeUpgradeSOSet)
            if (upgradeSO.upgradeType == UpgradeType.WarmingLamps)
                multiplier *= upgradeSO.timeMultiplier;
        return multiplier;
    }

    // called by CuttingCounter
    public static float GetChoppingSpeedMultiplier()
    {
        float multiplier = 1f;
        foreach (var upgradeSO in _activeUpgradeSOSet)
            if (upgradeSO.speedMultiplier < 1f)
                if (upgradeSO.upgradeType == UpgradeType.SharpKnives)
                    multiplier *= upgradeSO.speedMultiplier;
        return multiplier;
    }

    // called by StoveCounter
    public static float GetCookingSpeedMultiplier()
    {
        float multiplier = 1f;
        foreach (var upgradeSO in _activeUpgradeSOSet)
            if (upgradeSO.speedMultiplier < 1f)
                if (upgradeSO.upgradeType == UpgradeType.HotStoves)
                    multiplier *= upgradeSO.speedMultiplier;
        return multiplier;
    }

    // called by DeliveryManager
    public static int GetTipBonus(int baseValue)
    {
        float percentage = 0f;
        foreach (var upgradeSO in _activeUpgradeSOSet)
            if (upgradeSO.upgradeType == UpgradeType.GenerousTips)
                percentage += upgradeSO.tipPercentage;
        return Mathf.FloorToInt(baseValue * percentage);
    }

    // called by DeliveryManager
    public static int GetBonusOrderSlots()
    {
        int bonus = 0;
        foreach (var upgradeSO in _activeUpgradeSOSet)
            if (upgradeSO.upgradeType == UpgradeType.BiggerBoard)
                bonus += upgradeSO.bonusOrderSlots;
        return bonus;
    }
}
