using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLink : MonoBehaviour
{
    [SerializeField]
    float DamageModifier = 1f;
    [SerializeField]
    Health linkToHealth;

    public void TakeDamage(float damage,string source) {
        linkToHealth.TakeDamage(damage,source);
    }
}
