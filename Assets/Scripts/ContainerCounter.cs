using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayGrabbedObject;
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject()) return;
        
        KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
        
        OnPlayGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
