using System;
using TMPro;
using UnityEngine;

public class MoneyCountUI : MonoBehaviour
{
    private static readonly int AddPopupGreen = Animator.StringToHash("AddPopupGreen");
    private static readonly int AddPopupRed = Animator.StringToHash("AddPopupRed");
    
    [SerializeField] private TextMeshProUGUI totalMoneyEarnedText;
    [SerializeField] private TextMeshProUGUI moneyAdjustmentText;
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnOrderCompleted += DeliveryManager_OnOrderCompleted;
        DeliveryManager.Instance.OnOrderExpired += DeliveryManager_OnOrderExpired;
        
        UpdateVisual();
    }

    private void OnDestroy()
    {
        DeliveryManager.Instance.OnOrderCompleted -= DeliveryManager_OnOrderCompleted;
        DeliveryManager.Instance.OnOrderExpired -= DeliveryManager_OnOrderExpired;
    }

    private void DeliveryManager_OnOrderCompleted(object sender, DeliveryManager.OrderEventArgs e)
    {
        UpdateVisual();
        ShowMoneyAdjustment(e.order.GetRecipeSO().value);
    }
    
    private void DeliveryManager_OnOrderExpired(object sender, DeliveryManager.OrderEventArgs e)
    {
        UpdateVisual();
        ShowMoneyAdjustment(e.order.GetRecipeSO().cost);
    }

    private void UpdateVisual()
    {
        totalMoneyEarnedText.text = DeliveryManager.Instance.GetTotalMoneyEarned().ToString();
    }

    private void ShowMoneyAdjustment(int amount)
    {
        moneyAdjustmentText.text = amount > 0 ? "+" + amount : amount.ToString();
        _animator.SetTrigger(amount > 0 ? AddPopupGreen : AddPopupRed);
    }
}
