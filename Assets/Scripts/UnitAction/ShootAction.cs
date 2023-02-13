using System;
using System.Collections.Generic;
using Enemy;
using Grid;
using UnitClass;
using UnityEngine;

namespace UnitAction
{
    public class ShootAction : BaseAction
    {
        [Header("Variable")]
        [SerializeField] private int maxShootDistance = 7;
        [SerializeField] private float rotateSpeed = 10f;
        [SerializeField] private State state;
        [SerializeField] private float stateTimer;
        [SerializeField] private LayerMask obstacleLayerMask;
        private readonly float shootingStateTimer = 0.1f;
        private readonly float coolOffingStateTimer = 0.5f;
        private readonly float aimingStateTimer = 0.5f;

        private Unit targetUnit;
        private bool canShootBullet;

        public  event EventHandler<OnShootEventArgs> OnShoot;

        public void Update()
        {
            if (!isActive) return;

            stateTimer -= Time.deltaTime;

            switch (state)
            {
                case State.Aiming:
                    var aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                    transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                    break;
                
                case State.Shooting:
                    if (canShootBullet)
                    {
                        Shoot();
                        canShootBullet = false;
                    }
                    break;
                case State.CoolOff:
                    break;
            }

            if (stateTimer <= 0)
            {
                NextState();
            }
        }

        public override void TakeAction(GridPosition gridPosition, System.Action onActionComplete)
        {

            state = State.Aiming;
            
            stateTimer = aimingStateTimer;

            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            canShootBullet = true;
            
            ActionStart(onActionComplete);
        }

        private void Shoot()
        {
            targetUnit.Damage(40);
            OnStartShootInvoke();
        }

        private void NextState()
        {
            switch (state)
            {
                case State.Aiming:
                    state = State.Shooting;
                    stateTimer = shootingStateTimer;
                    break;
                
                case State.Shooting:
                    state = State.CoolOff;
                    stateTimer = coolOffingStateTimer;
                    break;
                
                case State.CoolOff:
                    ActionComplete();
                    break;
            }
        }

        public override List<GridPosition> GetValidActionGridPositionsList()
        {
            var unitOnGridPosition = unit.GetGridPosition();
            return GetValidActionGridPositionsList(unitOnGridPosition);
        }

        private List<GridPosition> GetValidActionGridPositionsList(GridPosition unitOnGridPosition)
        {
            var validActionGridPositionsList = new List<GridPosition>();

            for (var x = -maxShootDistance; x <= maxShootDistance; x++)
            {
                for (var z = -maxShootDistance; z <= maxShootDistance; z++)
                {
                    var offSetGridPosition = new GridPosition(x, z);
                    var testGridPosition = offSetGridPosition + unitOnGridPosition;
                    
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    // เดี่ยวมาทำต่อเอง
                    // อันนี้คือคือแบบ เดินของม้าแบบ fm
                    // int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    //
                    // if (testDistance > maxMoveDistance)
                    // {
                    //     continue;
                    // }
                    //
                    // validActionGridPositionsList.Add(testGridPosition);
                    // continue;
                    
                    var testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > maxShootDistance)
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


                    var unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unit.GetGridPosition());

                    var shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                    
                    const float unitShoulderHeight = 1.7f;
                    
                    var origin = unitWorldPosition + Vector3.up * unitShoulderHeight;
                    var distance = Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition());
                    
                    if (Physics.Raycast(origin ,shootDir, distance,obstacleLayerMask))
                    {
                        // Block by an obstacle
                        continue;
                    }
                    
                    validActionGridPositionsList.Add(testGridPosition);
                }
            }
            
            return validActionGridPositionsList;
        }

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            var unitAtGridPosition = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 100 + Mathf.RoundToInt((1 - unitAtGridPosition.GetHealthNormalized()) * 100f),
            };

        }

        public int GetTargetCountAtPosition(GridPosition gridPosition)
        {
            return GetValidActionGridPositionsList(gridPosition).Count;
        }

        public override string GetActionName()
        {
            return "Shoot";
        }

        protected virtual void OnStartShootInvoke()
        {
            OnShoot?.Invoke(this, new OnShootEventArgs(targetUnit, unit));
        }

        public Unit GetTargetUnit()
        {
            return targetUnit;
        }

        public int GetMaxShootDistance()
        {
            return maxShootDistance;
        }
    }

    internal enum State
    {
        Aiming,
        Shooting,
        CoolOff,
    }

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;

        public OnShootEventArgs(Unit targetUnit, Unit shootingUnit)
        {
            this.targetUnit = targetUnit;
            this.shootingUnit = shootingUnit;
        }
    }
}
