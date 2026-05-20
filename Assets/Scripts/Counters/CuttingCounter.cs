using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;
    
    public new static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    
    [SerializeField] private CuttingRecipeSO[] _cuttingRecipeSOArray;
    
    private int _cuttingProgress;
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            if (!HasRecipeWithInput(player.GetKitchenObject().KitchenObjectSO)) return;
            player.GetKitchenObject().SetParent(this);
            _cuttingProgress = 0;
                
            CuttingRecipeSO cuttingRecipe = GetCuttingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                NormalizedProgress = (float)_cuttingProgress / cuttingRecipe.cuttingProgressMax
            });
        }
        else if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetParent(player);
            
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                NormalizedProgress = 0f
            });
        }
        else if (HasKitchenObject() && player.HasKitchenObject())
        {
            if (!player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return;
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().KitchenObjectSO))
            {
                GetKitchenObject().DestroySelf();
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().KitchenObjectSO))
        {
            // _cuttingProgress++;
            int cuttingProgressAmount = Mathf.CeilToInt(1f / UpgradeManager.GetChoppingSpeedMultiplier());
            _cuttingProgress += Mathf.Max(1, cuttingProgressAmount);
            
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            OnCut?.Invoke(this, EventArgs.Empty);
            
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
            
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                NormalizedProgress = (float)_cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            
            if (_cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO kitchenObjectSO = GetOutputKitchenObject(GetKitchenObject().KitchenObjectSO);

                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(kitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputKitchenObject(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }

        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in _cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        
        return null;
    }
}
