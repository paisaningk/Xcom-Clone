using System;
using Grid;
using Sirenix.OdinInspector;
using Turn;
using UnitAction;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace UnitClass
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private BaseAction[] baseActionsArray;
        [SerializeField] private HealthSystem healthSystem;
        [SerializeField] private int maxActionPoints = 2;
        [SerializeField] private int actionPoints;
        [SerializeField] private bool isEnemy;
        private GridPosition gridPosition;

        public static event EventHandler OnAnyActionPointsChanged;
        public static event EventHandler OnAnyUnitSpawned;
        public static event EventHandler OnAnyUnitDead;

        public void OnValidate()
        {
            baseActionsArray = GetComponents<BaseAction>();
            healthSystem = GetComponent<HealthSystem>();
            ResetActionPoint();
            
        }

        public void Start()
        {
            gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.MoveUnitToGridPosition(this, gridPosition, gridPosition);

            TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
            healthSystem.OnDead += HealthSystem_OnDead;
            
            OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
        }

        public void Update()
        {
            
            var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition != gridPosition)
            {
                var oldGridPosition = gridPosition;
                gridPosition = newGridPosition;
                LevelGrid.Instance.MoveUnitToGridPosition(this,  oldGridPosition, newGridPosition);
            }
        }

        private void OnTurnChanged(object sender, EventArgs e)
        {
            if ((isEnemy && !TurnSystem.Instance.IsPlayerTurn()) || (!isEnemy && TurnSystem.Instance.IsPlayerTurn()) ) 
            {
                ResetActionPoint();
                OnAnyActionPointsChangedInvoke();
            }
        }

        public bool TrySpendActionPointToTakeAction(BaseAction baseAction)
        {
            if (CanSpendActionPointToTakeAction(baseAction))
            {
                SpendActionPoint(baseAction.GetActionPointsCost());
                return true;
            }

            return false;
        }

        public bool CanSpendActionPointToTakeAction(BaseAction baseAction)
        {
            if (actionPoints >= baseAction.GetActionPointsCost())
            {
                return true;
            }

            return false;
        }

        public T GetAction<T>() where T : BaseAction
        {
            foreach (var baseAction in baseActionsArray)
            {
                if (baseAction is T action)
                {
                    return action;
                }
            }

            return null;
        }

        public void Damage(int damageAmount)
        {
            healthSystem.Damage(damageAmount);
        }

        public Vector3 GetWordPosition()
        {
            return transform.position;
        }

        public void ResetActionPoint()
        {
            actionPoints = maxActionPoints;
        }

        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }

        public BaseAction[] GetBaseActionArray()
        {
            return baseActionsArray;
        }

        public float GetHealthNormalized()
        {
            return healthSystem.GetHealthNormalized();
        }

        public bool IsEnemy()
        {
            return isEnemy;
        }

        private void SpendActionPoint(int amount)
        {
            actionPoints -= amount;

            OnAnyActionPointsChangedInvoke();
        }

        public int GetActionPoint()
        {
            return actionPoints;
        }

        public override string ToString()
        {
            return gameObject.name;
        }

        private void HealthSystem_OnDead(object sender, EventArgs e)
        {
            LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
            LevelGrid.Instance.GetGridDebugObject(gridPosition).SetDebugText();
            
            Destroy(gameObject);
            
            OnAnyUnitDead.Invoke(this, EventArgs.Empty);
        }

        private void OnAnyActionPointsChangedInvoke()
        {
            OnAnyActionPointsChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
