using System.Collections.Generic;
using InterfaceClass;
using ObjectInGame.Door;
using UnitClass;

namespace Grid
{
    public class GridObject
    {
        private GridSystem<GridObject> gridSystem;
        private GridPosition gridPosition;
        private List<Unit> unitList;
        private IInteractable interactable;
        
        public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
            unitList = new List<Unit>();
        }

        public void AddUnit(Unit unit)
        {
            unitList.Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            unitList.Remove(unit);
        }

        public List<Unit> GetUnitList()
        {
            return unitList;
        }

        public bool HasAnyUnit()
        {
            return unitList.Count > 0;
        }
        
        public Unit GetUnit()
        {
            if (HasAnyUnit())
            {
                return unitList[0];
            }

            return null;
        }

        public override string ToString()
        {
            var unitString = "";
            foreach (var unit in unitList)
            {
                unitString += unit + "\n";
            }
            return gridPosition + "\n" + unitString;
        }

        public IInteractable GetDoor()
        {
            return interactable;
        }

        public void SetInteractable(IInteractable interactable)
        {
            this.interactable = interactable;
        }
    }
}