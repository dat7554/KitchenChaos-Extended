using UnityEngine;

public class UpgradePanelUI : MonoBehaviour
{
    [SerializeField] private UpgradeIconUI upgradeIconUIPrefab;
    
    private void Start()
    {
        foreach (var upgradeSO in UpgradeManager.GetActiveUpgrades())
        {
            UpgradeIconUI upgradeIconUI = Instantiate(upgradeIconUIPrefab, transform);
            upgradeIconUI.SetUpgradeSO(upgradeSO);
        }
    }
}
