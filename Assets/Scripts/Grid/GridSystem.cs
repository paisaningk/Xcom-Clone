using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Grid
{
    public class GridSystem<TGridObject> 
    {
        private int width;
        private int height;
        private float cellSize;
        private TGridObject[,] TGridObjectArray;
        private GridDebugObject[,] gridDebugObjectArray;

        public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            TGridObjectArray = new TGridObject[width,height];
            
            for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < height; z++)
                {
                    var gridPosition = new GridPosition(x, z);
                    TGridObjectArray[x, z] = createGridObject(this, gridPosition);
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
                    gridDebugObject.SetGridObject(GetTGridObject(gridPosition));
                    
                    
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

        public TGridObject GetTGridObject(GridPosition gridPosition)
        {
            return TGridObjectArray[gridPosition.x, gridPosition.z];
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