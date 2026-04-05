using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.GetKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
        }
    }
}
