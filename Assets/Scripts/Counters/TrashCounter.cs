using System;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;
    
    public new static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }
    
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                plateKitchenObject.RemoveAllIngredients();
            }
            else
            {
                player.GetKitchenObject().DestroySelf();
            }
            
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
