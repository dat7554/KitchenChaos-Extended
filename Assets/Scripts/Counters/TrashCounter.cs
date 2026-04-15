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
        if (player.GetKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
