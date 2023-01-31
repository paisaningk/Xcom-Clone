using Grid;
using UnityEngine;

namespace AStar
{
    public class Pathfinding : MonoBehaviour
    {
        [SerializeField] private Transform debugObjectPrefab;
        private int width;
        private int height;
        private float cellSize;
        private GridSystem<PathNode> gridSystem;

        private void Awake()
        {
            gridSystem = new GridSystem<PathNode>(10, 10, 2f, 
                (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
            gridSystem.CreateDebugObject(debugObjectPrefab, transform);

        }
    }
}