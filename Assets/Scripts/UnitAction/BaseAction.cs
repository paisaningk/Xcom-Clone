using System;
using System.Collections.Generic;
using Enemy;
using Grid;
using UnitClass;
using UnityEngine;

namespace UnitAction
{
    public abstract class BaseAction : MonoBehaviour
    {
        protected Unit unit;
        protected bool isActive;
        protected Action onActionComplete;

        public static event EventHandler OnAnyActionStart;
        public static event EventHandler OnAnyActionCompleted;

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
            
            OnAnyActionStartInvoke();
        }
        
        protected void ActionComplete()
        {
            isActive = false;
            onActionComplete();
            OnAnyActionCompletedInvoke();
        }

        private void OnAnyActionStartInvoke()
        {
            OnAnyActionStart?.Invoke(this, EventArgs.Empty);
        }

        private void OnAnyActionCompletedInvoke()
        {
            OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        }

        public Unit GetUnit()
        {
            return unit;
        }

        public EnemyAIAction GetBestEnemyAIAction()
        {
            var enemyAIActionList = new List<EnemyAIAction>();

            var validActionGridPosition = GetValidActionGridPositionsList();

            foreach (var gridPosition in validActionGridPosition)
            {
                var enemyAIAction = GetEnemyAIAction(gridPosition);
                
                enemyAIActionList.Add(enemyAIAction);
            }

            if (enemyAIActionList.Count > 0)
            {
                enemyAIActionList.Sort((((a, b) => b.actionValue - a.actionValue)));
                
                return enemyAIActionList[0];
            }
            else
            {
                return null;
            }
        }
        
        public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
    }
}
