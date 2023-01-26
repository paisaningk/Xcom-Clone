using System;
using System.Collections.Generic;
using UnitClass;
using UnityEngine;
using Utils;

namespace Grid
{
    public class LevelGrid : Singleton<LevelGrid>
    {
        [SerializeField] private Transform debugObjectPrefab;
        
        private GridSystem<GridObject> gridSystem;
        public event EventHandler OnAnyUnitMoveGridPosition;

        private void Awake()
        {
            gridSystem = new GridSystem<GridObject>(10, 10, 2f, 
                (g, gridPosition) => new GridObject(g,gridPosition));
            gridSystem.CreateDebugObject(debugObjectPrefab, transform);
        }

        public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        {
            var gridObject = gridSystem.GetTGridObject(gridPosition);
            gridObject.AddUnit(unit);
        }

        public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
        {
           return gridSystem.GetTGridObject(gridPosition).GetUnitList();
        }

        public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        {
            var gridObject = gridSystem.GetTGridObject(gridPosition);
            gridObject.RemoveUnit(unit);
        }
        
        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return gridSystem.GetGridPosition(worldPosition);
        }
        
        public Vector3 GetWordPosition(GridPosition gridPosition)
        {
            return gridSystem.GetWordPosition(gridPosition);
        }

        public void MoveUnitToGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
        {
            RemoveUnitAtGridPosition(fromGridPosition, unit);
            GetGridDebugObject(fromGridPosition).SetDebugText();
            
            AddUnitAtGridPosition(toGridPosition,unit);
            GetGridDebugObject(toGridPosition).SetDebugText();
            
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
            return gridSystem.GetTGridObject(gridPosition).HasAnyUnit();
        }
        
        public Unit GetUnitAtGridPosition(GridPosition gridPosition)
        {
            return gridSystem.GetTGridObject(gridPosition).GetUnit();
        }
    }
}