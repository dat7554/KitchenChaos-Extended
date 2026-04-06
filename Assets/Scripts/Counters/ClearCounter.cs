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
            if (player.GetKitchenObject() is PlateKitchenObject plateKitchenObject)
            {
                plateKitchenObject.AddIngredient(GetKitchenObject().KitchenObjectSO);
                GetKitchenObject().DestroySelf();
            }
        }
    }
}
