using System;
using System.Collections.Generic;
using Enemy;
using Grid;
using UnityEngine;
using UnityEngine.Video;

namespace UnitAction
{
    public class MoveAction : BaseAction
    {
        [Header("Variable")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float rotateSpeed = 4f;
        [SerializeField] private int maxMoveDistance = 4;

        public event EventHandler OnStartMoving;
        public event EventHandler OnStopMoving;
        
        private const float StopDistance = 0.1f;
        private Vector3 targetPosition;
        private GridPosition gridPosition;

        private void Awake()
        {
            targetPosition = transform.position;
        }
        
        private void Update()
        {
            if (!isActive) return;


            if (Vector3.Distance(transform.position, targetPosition) > StopDistance)
            {
                MoveToTarget();
            }
            else
            {
                ActionComplete();
                OnStopMovingInvoke();
            }
        }

        private void MoveToTarget()
        {
            var unitTransform = transform;
            var position = unitTransform.position;
            
            var moveDirection = (targetPosition - position).normalized;
            position += moveDirection * (moveSpeed * Time.deltaTime);
            
            unitTransform.position = position;
            unitTransform.forward = Vector3.Lerp(unitTransform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        }

        public override void TakeAction(GridPosition gridPosition, System.Action onMoveComplete)
        {
            targetPosition = LevelGrid.Instance.GetWordPosition(gridPosition);

            OnStartMovingInvoke();
            
            ActionStart(onMoveComplete);
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

                    if (unitOnGridPosition == testGridPosition)
                    {
                        continue;
                    }

                    if (LevelGrid.Instance.hasAnyUnitOnGridPosition(testGridPosition))
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
            var targetCountAtGridPosition = unit.GetShootAction().GetTargetCountAtPosition(gridPosition);
            
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = targetCountAtGridPosition * 10,
            };
            
        }


        public override string GetActionName()
        {
            return "Move";
        }

        protected virtual void OnStartMovingInvoke()
        {
            OnStartMoving?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnStopMovingInvoke()
        {
            OnStopMoving?.Invoke(this, EventArgs.Empty);
        }
    }
}