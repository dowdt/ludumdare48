using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField]
    float damage = 10f;

    float Cooldown = 0f;
    private void OnTriggerStay(Collider collider)
    {
        if (Cooldown > 0f)
        {
            Cooldown -= Time.fixedDeltaTime;
            return;
        }
        Cooldown = 0.3f;

        GameObject go = collider.gameObject;
        DamageLink link = go.GetComponent<DamageLink>();
        if (link)
            link.TakeDamage(damage, "Spike", transform.forward);

        Health h = go.GetComponent<Health>();
        if (h)
            h.TakeDamage(damage, "Spike", transform.forward);

    }
}
