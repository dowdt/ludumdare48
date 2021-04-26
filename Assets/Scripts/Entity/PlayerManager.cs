using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Health
{
    [SerializeField]
    CameraShaker shaker;

    [HideInInspector]
    public Movement move;

    public override void TakeDamage(float Amount, string source, Vector3 dir)
    {


        float angel = Vector3.Angle(transform.forward, -dir);
        if(Mathf.Abs(angel) > 90)
            Amount *= Mathf.Clamp(1f-Inventory.Items[Inventory.SelectedSlot].item.stopsDamage(),0,1f);

        base.TakeDamage(Amount,source,dir);



        shaker.Shake(Mathf.Clamp((5 + (Amount*0.6f) )*(Random.Range(0, 1) > 0.5 ? -1 : 1),-15,15)); 
    }

    private void Start()
    {
        move = GetComponent<Movement>();
    }

    public override void Die(string source)
    {

        GameManager.instance.PlayerDie();
        base.Die(source);
    }


}
