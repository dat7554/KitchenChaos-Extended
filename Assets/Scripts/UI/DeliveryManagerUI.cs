using System;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform orderUI;

    private void Awake()
    {
        orderUI.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnOrderSpawned += DeliveryManagerOnOrderSpawned;
    }

    private void OnDestroy()
    {
        DeliveryManager.Instance.OnOrderSpawned -= DeliveryManagerOnOrderSpawned;
    }

    private void DeliveryManagerOnOrderSpawned(object sender, DeliveryManager.OrderEventArgs e)
    {
        AddNewOrderUI(e.order);
    }
    
    private void AddNewOrderUI(Order order)
    {
        Transform recipeTransform = Instantiate(orderUI, container);
        recipeTransform.gameObject.SetActive(true);
        recipeTransform.GetComponent<OrderUI>().SetOrder(order);
    }
}
