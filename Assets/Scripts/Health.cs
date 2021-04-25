using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [HideInInspector]
    public float health;

    [SerializeField]
    float maxHealth = 100;

    public void TakeDamage(float Amount,string source) {
        health -= Amount;
        if (health <= 0f)
        {
            Die(source);
        }
    }

    public void Die(string source)
    {
        Debug.Log("Entity " + gameObject.name + " was killed by "+ source);
        health = 0f;

    }



    private void Awake()
    {
        health = maxHealth;
    }


}



