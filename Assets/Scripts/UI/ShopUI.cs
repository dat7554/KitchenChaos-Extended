using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private LevelSelectorUI levelSelectorUI;
    
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI budgetText;
    
    [Header("Buttons")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI purchaseButtonText;
    [SerializeField] private List<UpgradeButtonUI> upgradeButtonUIList;
    
#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool useTestBudget;
    [SerializeField] private int testBudgetAmount = 500;
#endif
    
    private UpgradeButtonUI _hoveredButton;
    private UpgradeButtonUI _selectedButton;
    private UpgradeButtonUI _lockedButton;
    
    private int _remainingBudget;

    private void Awake()
    {
        confirmButton.onClick.AddListener(() =>
        {
            SceneLoader.Instance.Load(GameModeSelector.GetGameModeSO().sceneEnum);
        });
        
        purchaseButton.onClick.AddListener(() =>
        {
            UpgradeButtonUI active = _hoveredButton ?? _selectedButton ?? _lockedButton;
            if (active == null) return;
            TryPurchase(active.GetUpgradeSO());
        });
    }

    private void Start()
    {
        levelSelectorUI.OnGameModeButtonClicked += LevelSelectorUI_OnGameModeButtonClicked;
        
        if (SceneLoader.Instance.IsSkipToShopEnable)
        {
            LevelSelectorUI_OnGameModeButtonClicked(this, null);
            RefreshShop();
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void OnDestroy()
    {
        levelSelectorUI.OnGameModeButtonClicked -= LevelSelectorUI_OnGameModeButtonClicked;
    }
    
    public void OnButtonHovered(UpgradeButtonUI button)
    {
        _hoveredButton = button;
        UpdateInfoPanel();
    }

    public void OnButtonUnhovered(UpgradeButtonUI button)
    {
        if (_hoveredButton == button) _hoveredButton = null;
        UpdateInfoPanel();
    }
    
    public void OnButtonSelected(UpgradeButtonUI button)
    {
        _selectedButton = button;
        _lockedButton = button;
        UpdateInfoPanel();
    }

    public void OnButtonDeselected(UpgradeButtonUI button)
    {
        if (_selectedButton == button) _selectedButton = null;
        UpdateInfoPanel();
    }
    
    public void OnButtonClicked(UpgradeButtonUI button)
    {
        _lockedButton = button; // lock on mouse click
        UpdateInfoPanel();
    }
    
    private void TryPurchase(UpgradeSO upgradeSO)
    {
        int cost = upgradeSO.GetCost(GameModeSelector.GetGameModeSO().difficultyKey);
        if (_remainingBudget < cost) return;

        _remainingBudget -= cost;
        UpgradeManager.AddUpgrade(upgradeSO);
        RefreshShop();
    }

    private void LevelSelectorUI_OnGameModeButtonClicked(object sender, EventArgs e)
    {
        _hoveredButton = null;
        _selectedButton = null;
        _lockedButton = null;
        
        string key = GameModeSelector.GetGameModeSO().difficultyKey;

#if UNITY_EDITOR
        _remainingBudget = useTestBudget ? testBudgetAmount : SaveManager.GetBudget(key);
#else
        _remainingBudget = SaveManager.GetBudget(key);
#endif

        RefreshShop();
        Show();
    }
    
    private void RefreshShop()
    {
        budgetText.text = _remainingBudget.ToString();
        
        foreach (var upgradeButton in upgradeButtonUIList)
        {
            bool alreadyOwned = UpgradeManager.HasUpgrade(upgradeButton.GetUpgradeSO());
            upgradeButton.UpdateState(_remainingBudget, alreadyOwned);
        }

        UpdateInfoPanel();
    }
    
    private void UpdateInfoPanel()
    {
        UpgradeButtonUI activeButton = _hoveredButton ?? _selectedButton ?? _lockedButton;
        
        upgradeNameText.text        = activeButton != null ? activeButton.GetUpgradeSO().displayName : "";
        upgradeDescriptionText.text = activeButton != null ? activeButton.GetUpgradeSO().description : "";
        upgradeCostText.text        = activeButton != null 
            ? $"Cost: {activeButton.GetUpgradeSO().GetCost(GameModeSelector.GetGameModeSO().difficultyKey).ToString()}" 
            : "";
        
        if (activeButton == null)
        {
            purchaseButton.gameObject.SetActive(false);
            return;
        }

        bool alreadyOwned = UpgradeManager.HasUpgrade(activeButton.GetUpgradeSO());
        bool canAfford    = _remainingBudget >= activeButton.GetUpgradeSO()
            .GetCost(GameModeSelector.GetGameModeSO().difficultyKey);

        purchaseButton.gameObject.SetActive(true);
        purchaseButton.interactable = canAfford && !alreadyOwned;
        
        if (alreadyOwned)       purchaseButtonText.text = "Owned";
        else if (!canAfford)    purchaseButtonText.text = "Can't Afford";
        else                    purchaseButtonText.text = "Purchase";
    }
    
    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);
}