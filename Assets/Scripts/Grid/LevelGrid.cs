using System.Collections.Generic;
using UnitClass;
using UnityEngine;
using Utils;

namespace Grid
{
    public class LevelGrid : Singleton<LevelGrid>
    {
        [SerializeField] private Transform debugObjectPrefab;
        private GridSystem gridSystem;

        private void Awake()
        {
            gridSystem = new GridSystem(10, 10, 2f);
            gridSystem.CreateDebugObject(debugObjectPrefab, transform);
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