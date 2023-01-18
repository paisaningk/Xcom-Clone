using System;
using System.Collections.Generic;
using Grid;
using UnitClass;
using UnityEngine;

namespace UnitAction
{
    public class ShootAction : BaseAction
    {
        [Header("Variable")]
        [SerializeField] private int maxMoveDistance = 7;
        [SerializeField] private float rotateSpeed = 10f;
        [SerializeField] private State state;
        [SerializeField] private float stateTimer;
        
        private readonly float shootingStateTimer = 0.1f;
        private readonly float coolOffingStateTimer = 0.5f;
        private readonly float aimingStateTimer = 0.5f;

        private Unit targetUnit;
        private bool canShootBullet;

        public event EventHandler<OnShootEventArgs> OnShoot;

        public void Update()
        {
            if (!isActive) return;

            stateTimer -= Time.deltaTime;

            switch (state)
            {
                case State.Aiming:
                    var aimDir = (targetUnit.GetWordPosition() - unit.GetWordPosition()).normalized;
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
            ActionStart(onActionComplete);
            
            state = State.Aiming;
            
            stateTimer = aimingStateTimer;

            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            canShootBullet = true;
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
            var validActionGridPositionsList = new List<GridPosition>();
            var unitOnGridPosition = unit.GetGridPosition();

            for (var x = -maxMoveDistance; x <= maxMoveDistance; x++)
            {
                for (var z = -maxMoveDistance; z <= maxMoveDistance; z++)
                {
                    var offSetGridPosition = new GridPosition(x, z);
                    var testGridPosition = offSetGridPosition + unitOnGridPosition;
                    
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    // เดี่ยวมาทำต่อเอง
                    // int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    //
                    // if (testDistance > maxMoveDistance)
                    // {
                    //     continue;
                    // }
                    //
                    // validActionGridPositionsList.Add(testGridPosition);
                    // continue;

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

        public override string GetActionName()
        {
            return "Shoot";
        }

        protected virtual void OnStartShootInvoke()
        {
            OnShoot?.Invoke(this, new OnShootEventArgs(targetUnit, unit));
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
