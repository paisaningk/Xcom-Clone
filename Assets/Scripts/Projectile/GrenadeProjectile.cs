using System;
using Destructible;
using Grid;
using UnitClass;
using Unity.Mathematics;
using UnityEngine;

namespace Projectile
{
    public class GrenadeProjectile : MonoBehaviour
    {
        [SerializeField] private int grenadeDamage = 40;
        [SerializeField] private float damageRadius = 4f;
        [SerializeField] private Transform grenadeExplodeVFXPrefab;
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private AnimationCurve arcYAnimationCurve;
        [SerializeField] private float maxHeight = 3;
        public static event EventHandler OnAnyGrenadeExploded;
        private Action onGrenadeBehActionComplete;
        private Vector3 targetGridPosition;
        private readonly float moveSpeed = 15;
        private readonly float reachedTargetDistance = 0.2f;
        private float totalDistance;
        private Vector3 positionXZ;
        
        private void Update()
        {
            var moveDir = (targetGridPosition - positionXZ).normalized;
            positionXZ += moveDir * (moveSpeed * Time.deltaTime);

            // this use with AnimationCurve to tell what time in curve
            // and AnimationCurve return value in time
            var height = totalDistance / maxHeight;
            var distance = Vector3.Distance(targetGridPosition, positionXZ);
            var distanceNormalized = 1 - distance / totalDistance;

            var positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * height;

            transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

            
            if (!(Vector3.Distance(targetGridPosition, transform.position) < reachedTargetDistance)) return;
            
            // check is hit 
            var overlapSphere = Physics.OverlapSphere(targetGridPosition, damageRadius);

            foreach (var variable in overlapSphere)
            {
                if (variable.TryGetComponent(out Unit targetUnit))
                {
                    targetUnit.Damage(grenadeDamage);
                }
                else if(variable.TryGetComponent(out DestructibleCrate destructibleCrate))
                {
                    destructibleCrate.Damage();
                }
            }

            // set parent trailRenderer before parent destroy myself
            trailRenderer.transform.parent = null;

            // spawn vfx 
            Instantiate(grenadeExplodeVFXPrefab, targetGridPosition + Vector3.up * 1, quaternion.identity);

            
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            
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

            // set xz use position for move in directly.
            positionXZ = transform.position;
            positionXZ.y = 0;
            
            totalDistance = Vector3.Distance(positionXZ, this.targetGridPosition);
        }
    }
}