using System;
using Grid;
using UnitClass;
using UnityEngine;

namespace Projectile
{
    public class GrenadeProjectile : MonoBehaviour
    {
        [SerializeField] private int grenadeDamage = 40;
        [SerializeField]private float damageRadius = 4f;
        private Action onGrenadeBehActionComplete;
        private Vector3 targetGridPosition;
        private readonly float moveSpeed = 15;
        private readonly float reachedTargetDistance = 0.2f;

        private void Update()
        {
            var moveDir = (targetGridPosition - transform.position);
            transform.position += moveDir * (moveSpeed * Time.deltaTime);

            if (!(Vector3.Distance(targetGridPosition, transform.position) < reachedTargetDistance)) return;
            
            var overlapSphere = Physics.OverlapSphere(targetGridPosition, damageRadius);

            foreach (var variable in overlapSphere)
            {
                if (variable.TryGetComponent(out Unit targetUnit))
                {
                    targetUnit.Damage(grenadeDamage);
                }
            }

            onGrenadeBehActionComplete();
                
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(targetGridPosition, damageRadius);
        }

        public void SetUp(GridPosition targetGridPosition , Action onGrenadeBehActionComplete)
        {
            this.onGrenadeBehActionComplete = onGrenadeBehActionComplete;
            this.targetGridPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        }
    }
}