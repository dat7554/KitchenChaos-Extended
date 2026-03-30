using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] _cuttingRecipeSOArray;
    
    private int _cuttingProgress;
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            if (HasRecipeWithInput(player.GetKitchenObject().KitchenObjectSO))
            {
                player.GetKitchenObject().SetParent(this);
                _cuttingProgress = 0;
            }
        }
        else if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetParent(player);
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().KitchenObjectSO))
        {
            _cuttingProgress++;
            
            CuttingRecipeSO cuttingRecipe = GetCuttingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
            if (_cuttingProgress >= cuttingRecipe.cuttingProgressMax)
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
