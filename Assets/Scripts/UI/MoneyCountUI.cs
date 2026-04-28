using System;
using TMPro;
using UnityEngine;

public class MoneyCountUI : MonoBehaviour
{
    private static readonly int AddPopup = Animator.StringToHash("AddPopup");
    
    [SerializeField] private TextMeshProUGUI totalMoneyEarnedText;
    [SerializeField] private TextMeshProUGUI moneyAdjustmentText;
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        
        UpdateVisual();
    }
    
    private void OnDestroy()
    {
        DeliveryManager.Instance.OnRecipeCompleted -= DeliveryManager_OnRecipeCompleted;
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, DeliveryManager.OnRecipeCompletedEventArgs e)
    {
        UpdateVisual();
        ShowMoneyAdjustment(e.recipeSO.value);
    }

    private void UpdateVisual()
    {
        totalMoneyEarnedText.text = DeliveryManager.Instance.GetTotalMoneyEarned().ToString();
    }

    private void ShowMoneyAdjustment(int value)
    {
        moneyAdjustmentText.text = "+" + value;
        _animator.SetTrigger(AddPopup);
    }
}
