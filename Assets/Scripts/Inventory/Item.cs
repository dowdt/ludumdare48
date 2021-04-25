using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public readonly string name;
    public Item(string _name) {
        name = _name;
    }

    public virtual Vector3 getOffsetPos() {
        return Vector3.zero;
    }
    public virtual Vector3 getOffsetRot()
    {
        return Vector3.zero;
    }
    public virtual void ItemInventoryUpdate()
    {
        


    }

    public virtual float stopsDamage()
    {
        return 0f;
    }
}
