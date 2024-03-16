using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyManagement : MonoBehaviour
{

    public PlayerManagement playerManagement;
    
    public float damage;
    public float health;

    //public Slider slider;  //ENEMY HEALTHBAR
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerManagement.GetDamage(damage);
        }
    }
}
