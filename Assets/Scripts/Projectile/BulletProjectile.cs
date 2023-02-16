using Unity.Mathematics;
using UnityEngine;

namespace Projectile
{
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField] private Transform bulletHitVFXPrefab;
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private float moveSpeed = 200f;
        private Vector3 targetPosition;
        public void Setup(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }

        private void Update()
        {
            var moveDir = (targetPosition - transform.position).normalized;

            var distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
            
            transform.position += moveDir * (moveSpeed * Time.deltaTime);
            
            var distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

            if (distanceBeforeMoving < distanceAfterMoving)
            {
                transform.position = targetPosition;
                
                trailRenderer.transform.parent = null;
                
                Destroy(gameObject);

                Instantiate(bulletHitVFXPrefab, transform.position, quaternion.identity);
            }

        }
    }
}