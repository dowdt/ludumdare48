using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [HideInInspector]
    public float health;

    [SerializeField]
    float regenSpeed = 0f;

    public readonly float maxHealth = 100;

    public virtual void TakeDamage(float Amount,string source) {
        health -= Amount;
        if (health <= 0f)
        {
            Die(source);
        }
    }

    public virtual void Die(string source)
    {
        Debug.Log("Entity " + gameObject.name + " was killed by "+ source);
        health = 0f;

    }

    private void Update()
    {
        if (health < maxHealth)
            health += Time.deltaTime * regenSpeed;
    }


    private void Awake()
    {
        health = maxHealth;
    }


}



