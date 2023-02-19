using System;
using System.Collections.Generic;
using Enemy;
using Grid;
using UnitClass;
using UnityEngine;

namespace UnitAction
{
    public class SwordAction : BaseAction
    {
        [SerializeField] private int maxSwordDistance = 1;
        [SerializeField] private int damage = 100;
        [SerializeField] private SwordState swordState;
        [SerializeField] private float stateTimer;

        private Unit targetUnit;
        private const float AfterHitStateTime = 0.5f;
        private const float BeforeHitStateTime = 0.7f;

        public static event EventHandler OnAnySwordHit;

        public event EventHandler OnSwordActionStarted;
        public event EventHandler OnSwordActionCompleted;

        public void Update()
        {
            if (!isActive) return;
            
            stateTimer -= Time.deltaTime;
            
            switch (swordState)
            {
                case SwordState.SwingSwordBeforeHit:
                    var aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                    var rotateSpeed = 10f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                    break;
                case SwordState.SwingSwordAfterHit:
                    break;
                default:
                    Debug.Log("SomeThing wrong in sword state");
                    break;
            }

            if (stateTimer <= 0)
            {
                NextState();
            }
        }
        
        private void NextState()
        {
            switch (swordState)
            {
                case SwordState.SwingSwordBeforeHit:
                    swordState = SwordState.SwingSwordAfterHit;
                    stateTimer = AfterHitStateTime;
                    targetUnit.Damage(damage);
                    OnAnySwordHitInvoke();
                    break;
                case SwordState.SwingSwordAfterHit:
                    OnSwordActionCompletedInvoke();
                    ActionComplete();
                    break;
                default:
                    Debug.Log("SomeThing wrong in sword state");
                    break;
            }
        }

        public override string GetActionName()
        {
            return "Sword";
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            Debug.Log("Sword Action");
            
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
           
            swordState = SwordState.SwingSwordBeforeHit;
            stateTimer = BeforeHitStateTime;

            OnSwordActionStartedInvoke();
            
            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionsList()
        {
            var validActionGridPositionsList = new List<GridPosition>();
            var unitOnGridPosition = unit.GetGridPosition();

            for (var x = -maxSwordDistance; x <= maxSwordDistance; x++)
            {
                for (var z = -maxSwordDistance; z <= maxSwordDistance; z++)
                {
                    var offSetGridPosition = new GridPosition(x, z);
                    var testGridPosition = offSetGridPosition + unitOnGridPosition;
                    
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }
                    
                    
                    if (!LevelGrid.Instance.hasAnyUnitOnGridPosition(testGridPosition))
                    {
                        // Grid Position is empty , no unit
                        continue;
                    }

                    var targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                    if (targetUnit.IsEnemy() == unit.IsEnemy())
                    {
                        //both unit on same team
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
                actionValue = 200
            };
        }

        public int GetMaxSwordDistance()
        {
            return maxSwordDistance;
        }

        protected virtual void OnSwordActionStartedInvoke()
        {
            OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSwordActionCompletedInvoke()
        {
            OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
        }

        private static void OnAnySwordHitInvoke()
        {
            OnAnySwordHit?.Invoke(null, EventArgs.Empty);
        }
    }

    public enum SwordState
    {
        SwingSwordBeforeHit,
        SwingSwordAfterHit,
        
    }
}