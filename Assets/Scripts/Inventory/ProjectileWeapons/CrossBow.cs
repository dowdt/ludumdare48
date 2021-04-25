using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBow : ProjectileWeapon
{
    public CrossBow() : base("CrossBow",false)
    {

    }

        public override Vector3 getBulletPos()
    {
        int i = getLineElementId();
        return line.transform.TransformPoint(new Vector3(line.GetPosition(i).x, line.GetPosition(i).y + 3, line.GetPosition(i).z));
    }

}
