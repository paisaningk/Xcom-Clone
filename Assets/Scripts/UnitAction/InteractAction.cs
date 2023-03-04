using System;
using System.Collections.Generic;
using Enemy;
using Grid;
using UnityEngine;

namespace UnitAction
{
    public class InteractAction : BaseAction
    {
        [SerializeField] private int maxInteractDistance = 1;
        public override string GetActionName()
        {
            return "Interact";
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }
            
            ActionComplete();
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            var doorAtGrid = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);
            doorAtGrid.Interact(OnInteractComplete);
            Debug.Log("Interact", this);
            
            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionsList()
        {
            var validActionGridPositionsList = new List<GridPosition>();
            var unitOnGridPosition = unit.GetGridPosition();

            for (var x = -maxInteractDistance; x <= maxInteractDistance; x++)
            {
                for (var z = -maxInteractDistance; z <= maxInteractDistance; z++)
                {
                    var offSetGridPosition = new GridPosition(x, z);
                    var testGridPosition = offSetGridPosition + unitOnGridPosition;
                    
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    var doorAtGrid = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);

                    if (doorAtGrid == null)
                    {
                        continue;
                    }

                    validActionGridPositionsList.Add(testGridPosition);
                }
            }
            
            return validActionGridPositionsList;
        }

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 0,
            };
        }
        
        public override int GetActionPointsCost()
        {
            return 0;
        }

        private void OnInteractComplete()
        {
            ActionComplete();
        }
    }
}