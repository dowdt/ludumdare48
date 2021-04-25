using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLink : MonoBehaviour
{
    [SerializeField]
    float DamageModifier = 1f;

    public Health linkToHealth;

    public void TakeDamage(float damage,string source,Vector3 dir) {
        linkToHealth.TakeDamage(damage,source, dir);
    }
}
