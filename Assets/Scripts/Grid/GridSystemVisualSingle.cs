using UnityEngine;

namespace Grid
{
    public class GridSystemVisualSingle : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;

        public void Show()
        {
            meshRenderer.enabled = true;
        }
        
        public void Close()
        {
            meshRenderer.enabled = false;
        }
    }
}