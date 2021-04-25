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

    Rigidbody rb;



    bool ishot = false;
    public void setHot() {
        ishot = true;
        lastPos = transform.position;

        Destroy(gameObject,60);
    }

    bool useRayCasts = false;


    private void OnCollisionEnter(Collision collision)
    {
        if (!ishot)
            return;

        float angleOffset = Vector3.Angle(collision.contacts[0].normal,-transform.forward);

        switch (onhit)
        {
            case OnHit.Destroy:
                Destroy(gameObject);
                break;
            case OnHit.Nothing:
                break;
            case OnHit.Stick:

                    DamageLink link = collision.gameObject.GetComponent<DamageLink>();
                    if (link)
                        link.TakeDamage(damage, "Arrow");
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
        //transform.forward =Vector3.Slerp(transform.forward, rb.velocity.normalized, Time.deltaTime);

        if (!ishot || !useRayCasts)
            return;
  
        RaycastHit hit;
        if (Physics.SphereCast(lastPos,0.1f, transform.position- lastPos, out hit, Vector3.Distance(lastPos, transform.position), RayLayers))
        {
            float angleOffset = Vector3.Angle(hit.normal, -transform.forward);
            Debug.DrawLine(hit.point,transform.position);

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

                        DamageLink link = hit.collider.GetComponent<DamageLink>();
                        if (link)
                            link.TakeDamage(damage,"Arrow");
                        

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

    private void Start()
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