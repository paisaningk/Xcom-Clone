using System;
using System.Collections.Generic;
using Enemy;
using Grid;
using Projectile;
using Unity.Mathematics;
using UnityEngine;

namespace UnitAction
{
    public class GrenadeAction : BaseAction
    {
        [SerializeField] private GrenadeProjectile grenadeProjectile;
        [SerializeField] private int maxThrowDistance = 7;
        
        public void Update()
        {
            if (!isActive) return;
            
            
        }

        public override string GetActionName()
        {
            return "Grenade";
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            var projectile = Instantiate(grenadeProjectile, unit.GetWorldPosition(), quaternion.identity);
            projectile.SetUp(gridPosition, OnGrenadeBehActionComplete);
            Debug.Log("Grenade Action");
            
            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionsList()
        {
            var validActionGridPositionsList = new List<GridPosition>();
            var unitOnGridPosition = unit.GetGridPosition();

            for (var x = -maxThrowDistance; x <= maxThrowDistance; x++)
            {
                for (var z = -maxThrowDistance; z <= maxThrowDistance; z++)
                {
                    var offSetGridPosition = new GridPosition(x, z);
                    var testGridPosition = offSetGridPosition + unitOnGridPosition;
                    
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    var testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > maxThrowDistance)
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

        private void OnGrenadeBehActionComplete()
        {
            ActionComplete();
        }
    }
}