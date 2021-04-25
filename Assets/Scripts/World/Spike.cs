using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    float damage = 10f;
    private void OnTriggerStay(Collider collider)
    {
        GameObject go = collider.gameObject;
        DamageLink link = go.GetComponent<DamageLink>();
        if (link)
            link.TakeDamage(damage, "Spike", transform.forward);

        Health h = go.GetComponent<Health>();
        if (h)
            h.TakeDamage(damage, "Spike", transform.forward);

    }
}
