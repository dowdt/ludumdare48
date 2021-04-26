using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaSpider : SpiderAI
{
    [Header("WebAttack")]

    [SerializeField]
    GameObject Web;
    [SerializeField]
    Transform throwFrom;
    [SerializeField]
    float ThrowForce = 20;
    [SerializeField]
    float ThrowCooldown = 10f;
    [SerializeField]
    float range = 20f;

    float ThrowCooldownTimer = 10f;

    public override void PlayerAttack()
    {

        base.PlayerAttack();
        float dis = Vector3.Distance(GameManager.playerInstance.transform.position, transform.position);

        if (range > dis && ThrowCooldownTimer < 0)
        {
            ThrowCooldownTimer = ThrowCooldown;

            Projectile go = Instantiate(Web,throwFrom.position,throwFrom.rotation).GetComponent<Projectile>();
            go.rb.AddForce((throwFrom.position- GameManager.playerInstance.transform.position).normalized *ThrowForce);
            go.setHot();

            Destroy(go.gameObject,20);
        }
        
        ThrowCooldownTimer -= Time.deltaTime;


    }
}
