using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private Order orderPrefab;
    
    public event EventHandler OnOrderSuccess;
    public event EventHandler OnOrderFailed;
    public event EventHandler<OrderEventArgs> OnOrderSpawned;
    public event EventHandler<OrderEventArgs> OnOrderCompleted;
    public event EventHandler<OrderEventArgs> OnOrderExpired;
    public class OrderEventArgs : EventArgs
    {
        public Order order;
    } 
    
    [SerializeField] private RecipeListSO recipeListSO;
    
    
    private List<Order> _waitingOrderList;
    private float _spawnPlateTimer;
    private float _spawnPlateTimerMax = 4f;
    private int _waitingOrderMax = 4;
    private int _successOrdersAmount;
    private int _totalAttemptsAmount;
    private int _moneyEarned;
    private int _moneyLost;
    private int _totalMoneyEarned;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than 1 instance of DeliveryManager in your scene.");
        }
        Instance = this;
        
        _waitingOrderList = new List<Order>();
    }

    private void Update()
    {
        _spawnPlateTimer -= Time.deltaTime;
        if (_spawnPlateTimer <= 0f)
        {
            _spawnPlateTimer = _spawnPlateTimerMax;

            if (GameManager.Instance.IsGamePlaying() && _waitingOrderList.Count < _waitingOrderMax)
            {
                SpawnOrder();
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        _totalAttemptsAmount++;
        
        foreach (var waitingOrder in _waitingOrderList)
        {
            if (!PlateMatchesRecipe(plateKitchenObject, waitingOrder.GetRecipeSO())) continue;
            
            _successOrdersAmount++;
            _moneyEarned += waitingOrder.GetRecipeSO().value;
            _totalMoneyEarned += waitingOrder.GetRecipeSO().value;
                    
            OnOrderCompleted?.Invoke(this, new OrderEventArgs() { order = waitingOrder });
            OnOrderSuccess?.Invoke(this, EventArgs.Empty);
                    
            RemoveOrder(waitingOrder);
            return;
        }

        OnOrderFailed?.Invoke(this, EventArgs.Empty);
    }

    public int GetSuccessRecipesAmount()
    {
        return _successOrdersAmount;
    }

    public int GetTotalAttemptsAmount()
    {
        return _totalAttemptsAmount;
    }
    
    public int GetMoneyEarned()
    {
        return _moneyEarned;
    }
    
    public int GetMoneyLost()
    {
        return _moneyLost;
    }

    public int GetTotalMoneyEarned()
    {
        return _totalMoneyEarned;
    }
    
    private void SpawnOrder()
    {
        RecipeSO waitingRecipeSO = recipeListSO.RecipeSOList[Random.Range(0, recipeListSO.RecipeSOList.Count)];
                
        Order order = Instantiate(orderPrefab, transform);
        order.SetRecipeSO(waitingRecipeSO);
        order.OnExpired += Order_OnExpired;
                
        _waitingOrderList.Add(order);
                
        OnOrderSpawned?.Invoke(this, new OrderEventArgs()
        {
            order = order
        });
    }

    private bool PlateMatchesRecipe(PlateKitchenObject plateKitchenObject, RecipeSO recipeSO)
    {
        if (plateKitchenObject.GetKitchenObjectSOList().Count != recipeSO.KitchenObjectSOList.Count) return false;
        
        foreach (var recipeKitchenObjectSO in recipeSO.KitchenObjectSOList)
        {
            bool ingredientFound = false;
                    
            foreach (var plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
            {
                if (recipeKitchenObjectSO == plateKitchenObjectSO)
                {
                    ingredientFound = true;
                    break;
                }
            }

            if (!ingredientFound)
            {
                return false;
            }
        }

        return true;
    }
    
    private void Order_OnExpired(object sender, EventArgs e)
    {
        Order order = sender as Order;
        if (order == null) return;

        _moneyLost += order.GetRecipeSO().cost;
        _totalMoneyEarned = Mathf.Max(0, _totalMoneyEarned + order.GetRecipeSO().cost);
        
        OnOrderExpired?.Invoke(this, new OrderEventArgs() { order = order });
        OnOrderFailed?.Invoke(this, EventArgs.Empty);
        
        RemoveOrder(order);
    }
    
    private void RemoveOrder(Order order)
    {
        order.OnExpired -= Order_OnExpired;
        
        _waitingOrderList.Remove(order);
        order.DestroySelf();
    }
}
