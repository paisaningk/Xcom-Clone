using System;
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
   }
}
