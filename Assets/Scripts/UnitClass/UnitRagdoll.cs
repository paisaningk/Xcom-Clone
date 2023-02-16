using UnityEngine;

namespace UnitClass
{
   public class UnitRagdoll : MonoBehaviour
   {
      [SerializeField] private Transform ragDollRootBone;

      public void Setup(Transform originalRootBone)
      {
         MatchAllChildTransform(originalRootBone, ragDollRootBone);

         var randomDir = new Vector3(Random.Range(-1f, 1f), 0 ,Random.Range(-1f, 1f));
         
         ApplyExplosionToRagDoll(ragDollRootBone, 500f, transform.position + randomDir, 10f);
      }

      private void MatchAllChildTransform(Transform root, Transform clone)
      {
         foreach (Transform child in root)
         {
            var cloneChild = clone.Find(child.name);
            if (cloneChild == null) continue;
            cloneChild.position = child.position;
            cloneChild.rotation = child.rotation;
               
            MatchAllChildTransform(child, cloneChild);
         }
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
