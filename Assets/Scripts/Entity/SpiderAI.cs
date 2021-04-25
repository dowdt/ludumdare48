using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpiderAI : Health
{
    SpiderMove SpiderMove;
    [SerializeField]
    GameObject RagDoll;

    [Header("Attacking")]

    [SerializeField]
    Transform attackFrom;

    [SerializeField]
    float damage = 20f;

    [SerializeField]
    float attackCooldown = 2f;
    float attackCooldownTimer;

    [SerializeField]
    float AttackRange = 2f;

    [SerializeField]
    AudioClip[] attacking;

    [SerializeField]
    AudioClip death;


    [Header("Tracking")]

    [SerializeField]
    LayerMask PlayerMaks;




    void Start()
    {
        SpiderMove = GetComponent<SpiderMove>();
    }


    public bool canSeePlayer()
    {
        RaycastHit hit;

        Debug.DrawLine(transform.position, transform.position+(GameManager.playerInstance.transform.position - transform.position)*100);
        if (Physics.Raycast(transform.position, GameManager.playerInstance.transform.position-transform.position, out hit, 100, PlayerMaks))
        {
            Debug.DrawLine(transform.position, hit.point,Color.black);

            if (hit.collider.gameObject == GameManager.playerInstance.gameObject)
                return true;
        }
        return false;
    }

    
    bool playedSound;
    public void Update()
    {
        
        if (canSeePlayer())
        {

            PlayerAttack();
        }


       
    }

    public virtual void PlayerAttack() {
        float dis = Vector3.Distance(GameManager.playerInstance.transform.position, transform.position);

        if (Vector3.Distance(GameManager.playerInstance.transform.position, attackFrom.position) < AttackRange)
        {
            if (!playedSound && attackCooldownTimer <= 0.1f)
            {
                audioSource.PlayOneShot(attacking[Random.Range(0, attacking.Length - 1)]);
                playedSound = true;
            }
            if (attackCooldownTimer <= 0f)
            {
                playedSound = false;
                attackCooldownTimer = attackCooldown;
                GameManager.playerInstance.GetComponent<PlayerManager>().TakeDamage(damage, "Spider", transform.position - GameManager.playerInstance.transform.position);
            }

        }
        attackCooldownTimer -= Time.deltaTime;

        SpiderMove.target = GameManager.playerInstance.transform.position;

    }




    public override void Die(string source)
    {
        base.Die(source);
        SpiderMove.rb.useGravity = true;
        SpiderMove.rb.drag = 0f;

        RagDoll.transform.SetParent(null);
        setupForRagdoll(RagDoll);

        audioSource.PlayOneShot(death);

        Destroy(gameObject);
    }
 
    void setupForRagdoll(GameObject ob)
    {
        Rigidbody rb = ob.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.isKinematic = false;
        }

        if (ob.GetComponent<SpiderLeg>())
            Destroy(ob.GetComponent<SpiderLeg>());

        if (ob.GetComponent<IKFabrik>())
            Destroy(ob.GetComponent<IKFabrik>());

        if (ob.GetComponent<SpiderLegGroup>())
            Destroy(ob.GetComponent<SpiderLegGroup>());

        if (ob.GetComponent<DamageLink>())
            Destroy(ob.GetComponent<DamageLink>());

        if (ob.GetComponent<Collider>())
            ob.GetComponent<Collider>().enabled = true;

        for (int i = 0; i < ob.transform.childCount; i++)
        {
            setupForRagdoll(ob.transform.GetChild(i).gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackFrom.position,AttackRange);
    }
}
