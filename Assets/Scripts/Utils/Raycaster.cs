using System.Runtime.CompilerServices;
using UnityEngine;

namespace Utils
{
    public class Raycaster : Singleton<Raycaster>
    {
        [SerializeField] private new Camera camera;
        
        private void Awake()
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
        }

        public (RaycastHit hitInfo, bool isHit) UseRaycastByMouse(LayerMask layerMask)
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            var isHit = Physics.Raycast(ray, out var hitInfo, float.MaxValue, layerMask);
            return (hitInfo, isHit);
        }
    }
}