using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnRecipeSpawned;
    public event EventHandler<OnRecipeCompletedEventArgs> OnRecipeCompleted;
    public class OnRecipeCompletedEventArgs : EventArgs
    {
        public RecipeSO recipeSO;
    } 
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    
    [SerializeField] private RecipeListSO recipeListSO;
    
    
    private List<RecipeSO> waitingRecipeSOList;
    private float _spawnPlateTimer;
    private float _spawnPlateTimerMax = 4f;
    private int _waitingRecipeMax = 4;
    private int _successRecipesAmount;
    private int _totalAttemptsAmount;
    private int _totalMoneyEarned;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than 1 instance of DeliveryManager in your scene.");
        }
        Instance = this;
        
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        _spawnPlateTimer -= Time.deltaTime;
        if (_spawnPlateTimer <= 0f)
        {
            _spawnPlateTimer = _spawnPlateTimerMax;

            if (GameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < _waitingRecipeMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.RecipeSOList[Random.Range(0, recipeListSO.RecipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);
                
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        _totalAttemptsAmount++;
        
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;
                
                foreach (var recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSOList)
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
                        plateContentsMatchesRecipe = false;
                    }
                }
                
                if (plateContentsMatchesRecipe)
                {
                    _successRecipesAmount++;
                    _totalMoneyEarned += waitingRecipeSO.value;
                    
                    OnRecipeCompleted?.Invoke(this, new OnRecipeCompletedEventArgs() { recipeSO = waitingRecipeSO });
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    
                    waitingRecipeSOList.Remove(waitingRecipeSO);
                    
                    return;
                }
            }
        }
        
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessRecipesAmount()
    {
        return _successRecipesAmount;
    }

    public int GetTotalAttemptsAmount()
    {
        return _totalAttemptsAmount;
    }

    public int GetTotalMoneyEarned()
    {
        return _totalMoneyEarned;
    }
}
