using UnityEngine;

namespace Utils
{
    public class MousePosition : Singleton<MousePosition>
    {
        [SerializeField] private GameObject mouseGameObject;
        [SerializeField] private LayerMask mouseLayerMask;

        public void Update()
        {
            mouseGameObject.transform.position = GetPointPosition();
        }

        public Vector3 GetPointPosition()
        {
            return Raycaster.Instance.UseRaycastByMouse(mouseLayerMask).hitInfo.point;
        }
    }
}