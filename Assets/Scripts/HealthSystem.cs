using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
   [SerializeField] private int maxHealth = 100;
   private int health = 100;

   public event EventHandler OnDead;
   public event EventHandler OnDamaged;

   private void Awake()
   {
      health = maxHealth;
   }

   public void Damage(int damageAmount)
   {
      health -= damageAmount;

      if (health < 0)
      {
         health = 0;
      }

      OnDamagedInvoke();

      if (health == 0)
      {
         Die();
      }

      #if UNITY_EDITOR
            Debug.Log(health);
      #endif
   }

   private void Die()
   {
      OnDeadInvoke();
   }

   private void OnDeadInvoke()
   {
      OnDead?.Invoke(this, EventArgs.Empty);
   }

   public float GetHealthNormalized()
   {
      return (float)health / maxHealth;
   }

   private void OnDamagedInvoke()
   {
      OnDamaged?.Invoke(this, EventArgs.Empty);
   }
}
