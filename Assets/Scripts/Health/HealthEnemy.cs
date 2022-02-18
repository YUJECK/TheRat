﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    public int health;
    public int maxHealth;
    GameManager cheese;
    public GameObject enemy;
    public RoomCloser roomCloser;

    private void Start()
    {
        cheese = FindObjectOfType<GameManager>();
    }
    public void TakeHit(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            cheese.SpawnCheese(enemy);
            Destroy(gameObject);
        }         
    }
    public void Heal(int bonusHealth)
    {
        health += bonusHealth;
        if(health > maxHealth)
        {
            health = maxHealth;
        }        
    }
    public void SetHealth(int NewMaxHealth, int NewHealth)
    {
        maxHealth = NewMaxHealth;
        health = NewHealth;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
    public void TakeAwayHealth(int TakeAwayMaxHealth, int TakeAwayHealth)
    {
        maxHealth -= TakeAwayMaxHealth;
        health -= TakeAwayHealth;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
    public void SetBonusHealth(int NewMaxHealth, int NewHealth)
    {
        Debug.Log("New Health");
        maxHealth += NewMaxHealth;
        health += NewHealth;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void OnDestroy()
    {
        if (roomCloser != null)
            roomCloser.enemyesCount--;
    }
}