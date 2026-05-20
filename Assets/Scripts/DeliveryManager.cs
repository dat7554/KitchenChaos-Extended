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
        public int valueChanged;
    } 
    
    [SerializeField, Tooltip("Fallback default")] private GameModeSO gameModeSO;
    
    private int MaxOrder => gameModeSO.maxOrders + UpgradeManager.GetBonusOrderSlots();
    
    private List<Order> _waitingOrderList;
    private float _spawnOrderTimer;
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

        // override with selected mode if one was chosen from the main menu
        if (GameModeSelector.GetGameModeSO() != null)
        {
            gameModeSO = GameModeSelector.GetGameModeSO();
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        
        _spawnOrderTimer -= Time.deltaTime;
        if (_spawnOrderTimer <= 0f)
        {
            _spawnOrderTimer = gameModeSO.spawnOrderInterval;

            if (_waitingOrderList.Count < MaxOrder)
            {
                SpawnOrder();
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        foreach (var waitingOrder in _waitingOrderList)
        {
            if (!PlateMatchesRecipe(plateKitchenObject, waitingOrder.GetRecipeSO())) continue;
            
            int baseValue = waitingOrder.GetRecipeSO().value;
            int tip = UpgradeManager.GetTipBonus(baseValue);
            int totalEarned = baseValue + tip;
            
            _moneyEarned += totalEarned;
            _totalMoneyEarned += totalEarned;
                    
            OnOrderCompleted?.Invoke(this, new OrderEventArgs() { order = waitingOrder, valueChanged = totalEarned});
            OnOrderSuccess?.Invoke(this, EventArgs.Empty);
                    
            RemoveOrder(waitingOrder);
            return;
        }

        OnOrderFailed?.Invoke(this, EventArgs.Empty);
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
        RecipeSO waitingRecipeSO = GetRandomRecipe();
                
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
    
    private RecipeSO GetRandomRecipe()
    {
        var pool = gameModeSO.recipePool;

        // Measure the total line length
        int totalWeight = 0;
        foreach (var entry in pool)
            totalWeight += entry.weight;

        // Throw a dart randomly anywhere on the line
        int roll = Random.Range(0, totalWeight);

        // Walk the segments until find where the dart landed
        int cumulative = 0;
        foreach (var entry in pool)
        {
            cumulative += entry.weight; // extend the running boundary
            if (roll < cumulative)      // dart is inside this segment
                return entry.recipeSO;
        }

        return pool[0].recipeSO; // fallback
    }
    
    private void Order_OnExpired(object sender, EventArgs e)
    {
        Order order = sender as Order;
        if (order == null) return;

        int cost = order.GetRecipeSO().cost;
        _moneyLost += cost;
        _totalMoneyEarned = Mathf.Max(0, _totalMoneyEarned + cost);
        
        OnOrderExpired?.Invoke(this, new OrderEventArgs() { order = order, valueChanged = cost });
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
