using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [HideInInspector]
    public float health;

    [SerializeField]
    float regenSpeed = 0f;

    [SerializeField]
    public AudioSource audioSource;

    bool isded = false;


    public float maxHealth = 100;

    public virtual void TakeDamage(float Amount,string source, Vector3 dir) {
        health -= Amount;
        if (health <= 0f)
        {
            if(!isded)
                Die(source);
            isded = true;
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



