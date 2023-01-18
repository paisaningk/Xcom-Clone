using System;
using System.Collections.Generic;
using Grid;
using UnitClass;
using UnityEngine;

namespace UnitAction
{
    public abstract class BaseAction : MonoBehaviour
    {
        protected Unit unit;
        protected bool isActive;
        protected System.Action onActionComplete;

        protected virtual void OnValidate()
        {
            unit = GetComponent<Unit>();
        }

        public abstract string GetActionName();

        public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

        public virtual bool IsValidActionGridPositions(GridPosition gridPosition)
        {
            var validActionGridPositionsList = GetValidActionGridPositionsList();
            return validActionGridPositionsList.Contains(gridPosition);
        }

        public abstract List<GridPosition> GetValidActionGridPositionsList();

        public virtual int GetActionPointsCost()
        {
            return 1;
        }

        protected void ActionStart(Action onActionComplete)
        {
            isActive = true;
            this.onActionComplete = onActionComplete;
        }
        
        protected void ActionComplete()
        {
            isActive = false;
            onActionComplete();
        }

    }
}
