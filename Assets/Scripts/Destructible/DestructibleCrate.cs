using System;
using Grid;
using UnityEngine;

namespace Destructible
{
    public class DestructibleCrate : MonoBehaviour
    {
        [SerializeField] private Transform createDestructiblePrefab;
        public static event EventHandler OnAnyDestroy;

        private GridPosition gridPosition;

        public void Start()
        { 
            gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        }

        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }


        public void Damage()
        {
            var position = transform.position;
            
            var destructiblePrefab = Instantiate(createDestructiblePrefab, position, Quaternion.identity);
            
            ApplyExplosionToRagDoll(destructiblePrefab, 150f, position, 10f);
            
            Destroy(gameObject);

            OnAnyDestroy?.Invoke(this, EventArgs.Empty);
        }
        
        private void ApplyExplosionToRagDoll(Transform root,float explosionForce , Vector3 explosionPosition, float explosionRange)
        {
            foreach (Transform child in root)
            {
                if (child.TryGetComponent(out Rigidbody childrigidbody))
                {
                    childrigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
                }
            
                ApplyExplosionToRagDoll(child,explosionForce, explosionPosition, explosionRange);
            }
        }
    }
}