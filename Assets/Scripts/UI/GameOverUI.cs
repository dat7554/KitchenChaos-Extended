using System;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyEarnedText;
    [SerializeField] private TextMeshProUGUI moneyLostText;
    [SerializeField] private TextMeshProUGUI totalMoneyEarnedText;
    
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
        if (GameManager.Instance.IsGameOver())
        {
            moneyEarnedText.text = DeliveryManager.Instance.GetMoneyEarned().ToString();
            moneyLostText.text = Mathf.Abs(DeliveryManager.Instance.GetMoneyLost()).ToString();
            totalMoneyEarnedText.text = DeliveryManager.Instance.GetTotalMoneyEarned().ToString();
            
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
