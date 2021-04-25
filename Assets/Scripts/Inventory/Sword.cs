using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{
    public Sword() : base("Sword")
    {
    }
    [SerializeField]
    Vector3 baseOffset;
    [SerializeField]
    Vector3 chargedOffset;
    [SerializeField]
    Vector3 BlockOffset;
    [SerializeField]
    Vector3 baseRotOffset;
    [SerializeField]
    Vector3 chargedRotOffset;
    [SerializeField]
    Vector3 BlockRotOffset;

    float state = 0f;

    [SerializeField]
    float damage = 60f;

    [SerializeField]
    AudioClip hitNoise;

    [SerializeField]
    float force = 100f;

    [SerializeField]
    float AttackRange = 1f;

    [SerializeField]
    float AttackSpeed = 4f;

    [SerializeField]
    LayerMask mask;

    float AttackCooldown = 0f;

    public void Update()
    {
        float _state = 0f;

        if (Input.GetMouseButtonDown(0) && AttackCooldown <= 0.1f) {
            AttackCooldown = 1;

            RaycastHit hit;

          
            if(Physics.SphereCast(GameManager.playerInstance.move.Cam.transform.position, 0.2f, GameManager.playerInstance.move.Cam.transform.forward, out hit, AttackRange, mask))
            {

                DamageLink link = hit.collider.GetComponent<DamageLink>();
           

                if (link)
                {
                  
                    link.TakeDamage(damage, "Sword", transform.forward);
  
   
                }

                Rigidbody tb = hit.rigidbody;
                if (tb && !tb.isKinematic)
                {
                    Vector3 dir = transform.forward;
                    dir.y *= 0.5f;
                    tb.AddForceAtPosition(dir * 55 * force,hit.point);
                    SoundManager.instance.PlayOneShot(hitNoise);
                }


            }
        }
   
        if (AttackCooldown > 0.1f)
        {
            AttackCooldown -= Time.deltaTime * AttackSpeed;
            _state = AttackCooldown;
        }
        

        if (Input.GetMouseButton(1))
            _state = -1;

        if (_state <= 0f)
            state = Mathf.Lerp(state, _state, Time.deltaTime * 10f);
        else
        {
            state = Mathf.Lerp(state, _state, Time.deltaTime * 30f);
        }
    }

    public override float stopsDamage() {
        return (state < -0.5f ? -state*0.5f : 0f);
    }


    public override Vector3 getOffsetPos()
    {
        return baseOffset + (state > 0f ? chargedOffset : BlockOffset) * state;
    }
    public override Vector3 getOffsetRot()
    {
        return baseRotOffset + (state > 0f ? chargedRotOffset : BlockRotOffset) * state;
    }



}
