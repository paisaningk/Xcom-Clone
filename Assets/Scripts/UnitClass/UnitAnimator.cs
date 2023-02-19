using System;
using Projectile;
using UnitAction;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace UnitClass
{
   public class UnitAnimator : MonoBehaviour
   {
      [SerializeField] private Animator animator;
      [SerializeField] private BulletProjectile bulletProjectilePrefab;
      [SerializeField] private Transform shootPointTransform;
      [SerializeField] private Transform riflePointTransform;
      [SerializeField] private Transform swordTransform;
      private readonly int isWalking = Animator.StringToHash("IsWalking");
      private readonly int shoot = Animator.StringToHash("Shoot");

      public void Awake()
      {
         if (TryGetComponent(out MoveAction moveAction))
         {
            moveAction.OnStartMoving += MoveActionOnOnStartMoving;
            moveAction.OnStopMoving += MoveActionOnOnStopMoving;
         }

         if (TryGetComponent(out ShootAction shootAction))
         {
            shootAction.OnShoot += ShootActionOnShoot;
         }

         if (TryGetComponent(out SwordAction swordAction))
         {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
         }
      }

      public void OnEnable()
      {
         EquipRifle();
      }

      private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
      {
         EquipRifle();
      }

      private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
      {
         animator.SetTrigger("SwordSlash");
         EquipSword();
      }

      private void ShootActionOnShoot(object sender, OnShootEventArgs e)
      {
         animator.SetTrigger(shoot);

         var position = shootPointTransform.position;
         
         var bulletProjectile = Instantiate(bulletProjectilePrefab, position, quaternion.identity);

         var targetUnitShootPosition = e.targetUnit.GetWorldPosition();

         targetUnitShootPosition.y = position.y;

         bulletProjectile.Setup(targetUnitShootPosition);
      }

      private void MoveActionOnOnStopMoving(object sender, EventArgs e)
      {
         animator.SetBool(isWalking, false);
      }

      private void MoveActionOnOnStartMoving(object sender, EventArgs e)
      {
         animator.SetBool(isWalking, true);
      }

      private void EquipSword()
      {
         swordTransform.gameObject.SetActive(true);
         riflePointTransform.gameObject.SetActive(false);
      }
      
      private void EquipRifle()
      {
         swordTransform.gameObject.SetActive(false);
         riflePointTransform.gameObject.SetActive(true);
      }
   }
}
