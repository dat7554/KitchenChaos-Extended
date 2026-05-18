using System;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyEarnedText;
    [SerializeField] private TextMeshProUGUI moneyLostText;
    [SerializeField] private TextMeshProUGUI totalMoneyEarnedText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI nextStarText;
    
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        
        Hide();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGameOver())
        {
            Hide();
            return;
        }
        
        string difficultyKey = GameModeSelector.GetGameModeSO().difficultyKey;
        int stars            = StarIconsUI.CalculateStars(out int nextStarMoney);
        int moneyEarned      = DeliveryManager.Instance.GetMoneyEarned();
        int totalMoneyEarned = DeliveryManager.Instance.GetTotalMoneyEarned();
        
        moneyEarnedText.text      = moneyEarned.ToString();
        moneyLostText.text        = Mathf.Abs(DeliveryManager.Instance.GetMoneyLost()).ToString();
        totalMoneyEarnedText.text = totalMoneyEarned.ToString();
        recordText.text           = SaveManager.GetBestScore(difficultyKey).ToString();
        nextStarText.text         = stars < 3 ? nextStarMoney.ToString() : "Max stars reached!";
        
        Show();
        
        SaveManager.SaveResult
            (
                difficultyKey, 
                totalMoneyEarned
            );
        
        SaveManager.SaveBudget
            (
                difficultyKey,
                moneyEarned
            );
        
        UpgradeManager.ResetUpgrades();
    }

    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);
}
