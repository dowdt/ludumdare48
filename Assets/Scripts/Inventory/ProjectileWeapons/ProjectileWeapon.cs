using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ProjectileWeapon : Item
{
    public ProjectileWeapon(string _name,bool _release = true) : base(_name)
    {
        release = _release;

    }

    [SerializeField]
    AudioClip shooting;

    [SerializeField]
    float force = 300f;

    [SerializeField]
    float chargeSpeed = 4f;


    float charge = 0;

    public GameObject bullet;
    [HideInInspector]
    public Projectile inSlingBullet = null;

    [SerializeField]
    float SlingStretch = 1f;

    [SerializeField]
    Vector3 baseOffset;
    [SerializeField]
    Vector3 chargedOffset;
    [SerializeField]
    Vector3 baseRotOffset;
    [SerializeField]
    Vector3 chargedRotOffset;


    bool release = true;

    private void OnDisable()
    {
        if (inSlingBullet)
            inSlingBullet.gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        if (inSlingBullet)
            inSlingBullet.gameObject.SetActive(true);

    }


    public override void ItemInventoryUpdate()
    {
        int i = getLineElementId();
        line.SetPosition(i, new Vector3(line.GetPosition(i).x, charge * SlingStretch, line.GetPosition(i).z));

        if (inSlingBullet != null)
        {
            inSlingBullet.transform.position = getBulletPos();
            inSlingBullet.transform.rotation = getBulletRot();
        }

        
    }
    public virtual Vector3 getBulletPos()
    {
        int i = getLineElementId();
        return line.transform.TransformPoint(new Vector3(line.GetPosition(i).x, line.GetPosition(i).y, line.GetPosition(i).z));
    }
    public virtual Quaternion getBulletRot()
    {
        int i = getLineElementId();
        return transform.rotation;
    }
    public int getLineElementId() {
        return ((line.positionCount - 1) / 2);
        
    }


    float cooldown = 0;
    void Shoot() {
        if (inSlingBullet != null && cooldown <= 0)
        {
            SoundManager.instance.PlayOneShot(shooting);
            Rigidbody bulletRb = inSlingBullet.gameObject.GetComponent<Rigidbody>();
            inSlingBullet.setHot();
            if (bulletRb == null)
                Destroy(inSlingBullet);
            bulletRb.AddForce(charge * charge * force * transform.forward * 10);

        }
        cooldown = 0.2f;
        inSlingBullet = null;
    }

    public void Update()
    {
        if (cooldown > 0f)
        {
            if(charge <= 0.1f)
                cooldown -= Time.deltaTime;
            charge = Mathf.Lerp(charge, 0, Time.deltaTime * 30f);
            return;
        }
        if (cooldown <= 0f)
        {


            if (inSlingBullet == null && charge > 0.1f)
            {
                inSlingBullet = Instantiate(bullet).GetComponent<Projectile>();
            }

            if (!release && Input.GetMouseButtonDown(1) && charge > 0.9f)
            {
                Shoot();
                return;
            }

            if (Input.GetMouseButton(1))
            {
                charge = Mathf.Lerp(charge, 1, Time.deltaTime * chargeSpeed * GameManager.playerInstance.move.getInWebSpeed());
            }
            else
            {
                if (release || charge <= 0.9f)
                    Shoot();
                
            }


        }


    }

    public override Vector3 getOffsetPos()
    {
        return baseOffset+chargedOffset*charge;
    }
    public override Vector3 getOffsetRot()
    {
        return baseRotOffset + chargedRotOffset * charge;
    }

    public LineRenderer line;
}
