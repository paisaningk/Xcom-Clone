using System;
using System.Collections.Generic;
using Enemy;
using Grid;
using UnityEngine;

namespace UnitAction
{
    public class SpinAction : BaseAction
    {
        private float totalSpinAmount;

        public void Update()
        {
            if (!isActive) return;
            
            var spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

            totalSpinAmount += spinAddAmount;
            
            if (totalSpinAmount >= 360)
            {
                ActionComplete();
            }
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            totalSpinAmount = 0;
            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionsList()
        {
            var unitGridPosition = unit.GetGridPosition();

            return new List<GridPosition>
            {
                unitGridPosition
            };
        }

        public override int GetActionPointsCost()
        {
            return 2;
        }

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 0,
            };
        }

        public override string GetActionName()
        {
            return "Spin";
        }
    }
}