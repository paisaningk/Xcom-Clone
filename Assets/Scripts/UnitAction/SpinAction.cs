using System;
using System.Collections.Generic;
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
            ActionStart(onActionComplete);
            totalSpinAmount = 0;
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

        public override string GetActionName()
        {
            return "Spin";
        }
    }
}