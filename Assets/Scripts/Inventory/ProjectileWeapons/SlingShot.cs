using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : ProjectileWeapon
{
    public SlingShot() : base("SlingShot")
    {

    }
    public override Vector3 getBulletPos()
    {
        int i = getLineElementId();
        return line.transform.TransformPoint(new Vector3(line.GetPosition(i).x, line.GetPosition(i).y - inSlingBullet.transform.localScale.y * 2 - 0.01f, line.GetPosition(i).z));
    }



}
