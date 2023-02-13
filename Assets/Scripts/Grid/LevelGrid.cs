using System;
using System.Collections.Generic;
using AStar;
using UnitClass;
using UnityEngine;
using Utils;

namespace Grid
{
    public class LevelGrid : Singleton<LevelGrid>
    {
        [SerializeField] private Transform debugObjectPrefab;
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float cellSize;
        [SerializeField] private LayerMask obstaclesLayerMask;
        [SerializeField] private bool isbug = false;
        
        private GridSystem<GridObject> gridSystem;
        public event EventHandler OnAnyUnitMoveGridPosition;
        
        private void OnDrawGizmos()
        {
            if (!isbug) return;
            gridSystem = new GridSystem<GridObject>(width, height, cellSize, 
                (g, gridPosition) => new GridObject(g,gridPosition));

            for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < height; z++)
                {
                    var gridPosition = new GridPosition(x, z);

                    var position = gridSystem.GetWordPosition(gridPosition);

                    position.y = 0.01f;
                    
                    var rayCastOffSetDistance = 5;
                    var worldPosition = GetWorldPosition(gridPosition) + Vector3.down * rayCastOffSetDistance;
                    Gizmos.color = Physics.Raycast(worldPosition, Vector3.up, rayCastOffSetDistance * 2, obstaclesLayerMask) 
                        ? Color.red : Color.grey;
                    
                    Gizmos.DrawCube(position , new Vector3(1,0,1) * (cellSize - 0.5f)); 
                }
            }

        }

        private void Awake()
        {
            gridSystem = new GridSystem<GridObject>(width, height, cellSize, 
                (g, gridPosition) => new GridObject(g,gridPosition));
            //gridSystem.CreateDebugObject(debugObjectPrefab, transform);
        }

        private void Start()
        {
            //set pathfinding run before anything
            PathFinding.Instance.Setup(width, height, cellSize);
        }

        public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        {
            var gridObject = gridSystem.GetGridObject(gridPosition);
            gridObject.AddUnit(unit);
        }

        public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
        {
           return gridSystem.GetGridObject(gridPosition).GetUnitList();
        }

        public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        {
            var gridObject = gridSystem.GetGridObject(gridPosition);
            gridObject.RemoveUnit(unit);
        }
        
        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return gridSystem.GetGridPosition(worldPosition);
        }
        
        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return gridSystem.GetWordPosition(gridPosition);
        }

        public void MoveUnitToGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
        {
            // need debug un comment
            RemoveUnitAtGridPosition(fromGridPosition, unit);
            //GetGridDebugObject(fromGridPosition).SetDebugText();

            AddUnitAtGridPosition(toGridPosition,unit);
            //GetGridDebugObject(fromGridPosition).SetDebugText();
            
            OnAnyUnitMoveGridPosition?.Invoke(this, EventArgs.Empty);
        }
        
        public int GetWidth()
        {
            return gridSystem.GetWidth();
        }
        
        public int GetHeight()
        {
            return gridSystem.GetHeight();
        }

        public GridDebugObject GetGridDebugObject(GridPosition gridPosition)
        {
            return gridSystem.GetGridDebugObject(gridPosition);
        }

        public bool IsValidGridPosition(GridPosition gridPosition)
        {
            return gridSystem.IsValidGridPosition(gridPosition);
        }

        public bool hasAnyUnitOnGridPosition(GridPosition gridPosition)
        {
            return gridSystem.GetGridObject(gridPosition).HasAnyUnit();
        }
        
        public Unit GetUnitAtGridPosition(GridPosition gridPosition)
        {
            return gridSystem.GetGridObject(gridPosition).GetUnit();
        }
    }
}