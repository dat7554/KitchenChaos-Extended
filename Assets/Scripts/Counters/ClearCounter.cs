using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            player.GetKitchenObject().SetParent(this);
        }
        else if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetParent(player);
        }
        else if (HasKitchenObject() && player.HasKitchenObject())
        {
            // Case 1: Player has plate
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                if (plateKitchenObject.TryAddIngredient(GetKitchenObject().KitchenObjectSO))
                {
                    GetKitchenObject().DestroySelf();
                }
            }
            // Case 2: Counter has plate
            else if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
            {
                if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().KitchenObjectSO))
                {
                    player.GetKitchenObject().DestroySelf();
                }
            }
        }
    }
}
