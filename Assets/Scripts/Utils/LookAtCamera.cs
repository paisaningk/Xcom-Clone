using UnityEngine;

namespace Utils
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private bool inevet;
        private Transform cameraTransform;

        private void Awake()
        {
            if (Camera.main != null) cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            if (inevet)
            {
                var dirToCamera = (cameraTransform.position - transform.position).normalized;
                transform.LookAt( transform.position + dirToCamera * -1);
            }
            else
            {
                transform.LookAt(cameraTransform);    
            }
            
        }
    }
}
