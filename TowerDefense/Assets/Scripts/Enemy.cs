using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float StartHealth = 100;
    public float health = 100;
    public int points = 100;
    public int payValue = 50;

    [Header("Unity Stuff")]
    public Image healthBar;
    
    void Start()
    {
        health = StartHealth;

        GameObject.Find("pCone2").GetComponent<Renderer>().material.color = Color.red;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        

        healthBar.fillAmount = health / StartHealth;

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameMaster.Instance.currency = GameMaster.Instance.currency + payValue;
        GameMaster.Instance.points = GameMaster.Instance.points + points;
        Destroy(gameObject);
    }
}
