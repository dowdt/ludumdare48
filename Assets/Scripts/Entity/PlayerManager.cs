using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Health
{
    [SerializeField]
    CameraShaker shaker;

    public virtual void TakeDamage(float Amount, string source)
    {
        base.TakeDamage(Amount,source);



        shaker.Shake((5 + (Amount*0.6f) )*(Random.Range(0, 1) == 1 ? -1 : 1)); 
    }

    

    public virtual void Die(string source)
    {
        base.Die(source);
    }


}
