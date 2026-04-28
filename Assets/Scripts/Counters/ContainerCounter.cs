using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayGrabbedObject;
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    public override void Interact(Player player)
    {
        bool canGrab = true;
        
        if (player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            canGrab = plateKitchenObject.TryAddIngredient(kitchenObjectSO);
        }
        else if (!player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
        }
        
        if (canGrab)
           OnPlayGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
