using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Projectile : MonoBehaviour
{
    [SerializeField]
    OnHit onhit;



    [SerializeField]
    Vector3 centerOfGravity;



    [SerializeField]
    float damage = 10;

    bool ishot = false;
    public void setHot() {
        ishot = true;
    }


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
                if (angleOffset < 60)
                {
                    Destroy(gameObject, 20);
                    Destroy(this.GetComponent<Rigidbody>());
                    gameObject.transform.SetParent(collision.collider.transform);
                    Destroy(this);
                    Destroy(this.GetComponent<Collider>());
                }

                break;
            default:
                break;
        }
    }
    private void Start()
    {
        this.GetComponent<Rigidbody>().centerOfMass = centerOfGravity;

    }
}
[System.Serializable]
public enum OnHit
{
    Destroy,Nothing,Stick
}