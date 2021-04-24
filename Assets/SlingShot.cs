using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : Item
{
    public SlingShot() : base("SlingShot")
    {
        
    }
    [SerializeField]
    float force = 150f;
    [Range(0,1)]
    public float charge = 1;

    public GameObject bullet;

    public GameObject inSlingBullet = null;
    public override void ItemInventoryUpdate()
    {
        int i = (line.positionCount - 1) / 2;
        line.SetPosition(i, new Vector3(line.GetPosition(i).x, charge * 0.5f, line.GetPosition(i).z));

        bool isFiring = (Input.GetMouseButton(1));
        if (isFiring)
        {
            if (inSlingBullet == null)
                inSlingBullet = Instantiate(bullet);
            inSlingBullet.transform.position = line.transform.TransformPoint(new Vector3(line.GetPosition(i).x, line.GetPosition(i).y-inSlingBullet.transform.localScale.y*2, line.GetPosition(i).z));
            inSlingBullet.transform.rotation = transform.rotation;
            charge = Mathf.Lerp(charge, 1, Time.deltaTime * 4f);
        }
        else
        {
            if (inSlingBullet != null)
            {
                Rigidbody bulletRb = inSlingBullet.GetComponent<Rigidbody>();
                if (bulletRb == null)
                    Destroy(inSlingBullet);
                bulletRb.AddForce(charge* charge * force*transform.forward*10);
            }
            inSlingBullet = null;
            charge = Mathf.Lerp(charge, 0, Time.deltaTime * 30f);
        }


        
    }

    public override Vector3 getOffsetPos()
    {
        return new Vector3(-0.2f,0, -charge * 0.03f);
    }
    public override Vector3 getOffsetRot()
    {
        return new Vector3(charge * -12, charge * -12 + 10, charge * 10f+12);
    }

    public LineRenderer line;
}
