using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebBall : Projectile
{
    [SerializeField]
    float WebTime = 8f;
    public override void onHit(GameObject collider)
    {
        base.onHit(collider);

   

        if (collider == GameManager.playerInstance.gameObject)
        {
      
            if(Inventory.Items[Inventory.SelectedSlot].item.stopsDamage() <= 0f)
                 GameManager.playerInstance.move.setInWeb(WebTime);
        }
    }
}
