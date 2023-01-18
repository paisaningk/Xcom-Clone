using UnityEngine;

namespace Grid
{
    public class GridSystem
    {
        private int width;
        private int height;
        private float cellSize;
        private GridObject[,] gridObjectArray;
        private GridDebugObject[,] gridDebugObjectArray;

        public GridSystem(int width, int height, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            gridObjectArray = new GridObject[width,height];
            
            for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < height; z++)
                {
                    var gridPosition = new GridPosition(x, z);
                    gridObjectArray[x, z] = new GridObject(this, gridPosition);
                }
            }
        }

        public void CreateDebugObject(Transform debugPrefab , Transform parenTransform)
        {
            gridDebugObjectArray = new GridDebugObject[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < height; z++)
                {
                    var gridPosition = new GridPosition(x, z);
                    
                    var debug = Object.Instantiate(debugPrefab, GetWordPosition(gridPosition), Quaternion.identity);
                    
                    debug.parent = parenTransform;
                    
                    var gridDebugObject = debug.GetComponent<GridDebugObject>();
                    
                    gridDebugObjectArray[x, z] = gridDebugObject;
                    gridDebugObject.SetGridPosition(GetGridObject(gridPosition));
                    
                    
                }
            }
        }

        public Vector3 GetWordPosition(GridPosition gridPosition)
        {
            return new Vector3(gridPosition.x,0 ,gridPosition.z) * cellSize;
        }

        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return new GridPosition
            (
                Mathf.RoundToInt(worldPosition.x / cellSize),
                Mathf.RoundToInt(worldPosition.z / cellSize)
            );
        }

        public GridObject GetGridObject(GridPosition gridPosition)
        {
            return gridObjectArray[gridPosition.x, gridPosition.z];
        }

        public GridDebugObject GetGridDebugObject(GridPosition gridPosition)
        {
            return gridDebugObjectArray[gridPosition.x, gridPosition.z];
        }

        public bool IsValidGridPosition(GridPosition gridPosition)
        {
            return gridPosition.x >= 0 && 
                   gridPosition.z >= 0 && 
                   gridPosition.x < width && 
                   gridPosition.z < height;
        }

        public int GetWidth()
        {
            return width;
        }
        
        public int GetHeight()
        {
            return height;
        }
    }
}