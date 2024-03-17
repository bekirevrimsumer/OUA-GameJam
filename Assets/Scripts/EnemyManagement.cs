using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyManagement : MonoBehaviour
{

    public PlayerManager playerManagement;
    public PlayerMovementAdvanced playerMovement;
    public Animator animator;
    
    public float damage;
    public float health;
    
    private NavMesh navMesh;

    private void Start()
    {
        navMesh = GetComponent<NavMesh>();
        animator = GetComponent<Animator>();
    }
    
    // private void OnCollisionEnter(Collision other)
    // {
    //     Debug.Log("Collision");
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Player hit");
    //         playerManagement.GetDamage(damage);
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && navMesh.isFighting == true)
        {
            var random = UnityEngine.Random.Range(0, 5);
            if(random == 0)
                playerManagement.GetDamage(damage);
        }
    }

    public void GetDamage(float damage)
    {
        if ((health - damage) >= 0)
        {
            health -= damage;
        }
        else
        {
            health = 0;
            Die();
        }
    }
    
    public void Die()
    {
        animator.SetBool("isDead", true);
        Destroy(gameObject,2);
    }
}
