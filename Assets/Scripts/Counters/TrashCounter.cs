using System;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;
    
    public override void Interact(Player player)
    {
        if (player.GetKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
