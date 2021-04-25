using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Projectile : MonoBehaviour
{
    [SerializeField]
    OnHit onhit;

    [SerializeField]
    float damage = 50f;

    [SerializeField]
    LayerMask RayLayers;

    [SerializeField]
    Vector3 centerOfGravity;


    [HideInInspector]
    public Rigidbody rb;



    bool ishot = false;
    public void setHot() {
        ishot = true;
        lastPos = transform.position;

        Destroy(gameObject,60);
    }

    bool useRayCasts = false;


    public virtual void onHit(GameObject collider)
    {

        switch (onhit)
        {
            case OnHit.Destroy:
                Destroy(gameObject);
                break;
            case OnHit.Nothing:
                break;
            case OnHit.Stick:

                DamageLink link = collider.GetComponent<DamageLink>();
                if (link)
                    link.TakeDamage(damage, "Projectile", transform.forward);

                Health h = collider.GetComponent<Health>();
                if (h)
                    h.TakeDamage(damage, "Projectile", transform.forward);


                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!ishot)
            return;

        Debug.Log(collision);
        onHit(collision.gameObject);


        switch (onhit)
        {
            case OnHit.Destroy:
                Destroy(gameObject);
                break;
            case OnHit.Nothing:
                break;
            case OnHit.Stick:

                    
                    Destroy(this.GetComponent<Rigidbody>());
                    gameObject.transform.SetParent(collision.collider.gameObject.transform);
                    Destroy(this);
                    Destroy(this.GetComponent<Collider>());
                

                break;
            default:
                break;
        }
    }



    public void Update()
    {

        if (!ishot || !useRayCasts)
            return;
  
        RaycastHit hit;
        if (Physics.SphereCast(lastPos,0.1f, transform.position- lastPos, out hit, Vector3.Distance(lastPos, transform.position), RayLayers))
        {

            onHit(hit.collider.gameObject);
          

            switch (onhit)
            {
                case OnHit.Destroy:
                    Destroy(gameObject);
                    break;
                case OnHit.Nothing:
                    break;
                case OnHit.Stick:

                        if (hit.rigidbody)
                            hit.rigidbody.AddForceAtPosition(rb.velocity*80f,hit.point);

                       

                        Destroy(gameObject, 20);
                        Destroy(this.GetComponent<Rigidbody>());

                        Destroy(this);
                        if(this.GetComponent<Collider>())
                            Destroy(this.GetComponent<Collider>());
                         
                      
                        transform.SetParent(hit.collider.transform);
                    transform.position = hit.point;

                    break;
                default:
                    break;
            }
        }

        lastPos = transform.position;
    }
    Vector3 lastPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        useRayCasts = (GetComponent<Collider>() == null);
        rb.centerOfMass = centerOfGravity;

    }
}
[System.Serializable]
public enum OnHit
{
    Destroy,Nothing,Stick
}