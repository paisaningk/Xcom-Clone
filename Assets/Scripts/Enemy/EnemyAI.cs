using System;
using Grid;
using Turn;
using UnitClass;
using UnityEngine;
using Utils;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float setTimer = 2f;
        [SerializeField] private State state;
        private float timer;

        public void Start()
        {
            state = State.WaitingForEnemyTurn;
            
            TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
        }

        public void Update()
        {
            if (TurnSystem.Instance.IsPlayerTurn()) return;
            
            switch (state)
            {
                case State.WaitingForEnemyTurn:
                    break;
                case State.TakingTurn:
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        if (TryTakeEnemyAIAction(SetStateTakingTurn))
                        {
                            state = State.Busy;
                        }
                        else
                        {
                            //no more enemy have action to use
                            TurnSystem.Instance.NextTurn();
                        }
                    }
                    break;
                case State.Busy:
                    
                    break;
            }
        }

        private void OnTurnChanged(object sender, EventArgs e)
        {
            if (TurnSystem.Instance.IsPlayerTurn()) return;
            timer = setTimer;
            state = State.TakingTurn;
        }

        private bool TryTakeEnemyAIAction(Action onEnemyAIAction)
        {
            //call action here wtf code mokey
            foreach (var enemyUnit in UnitManager.Instance.GetEnemyUnitList())
            {
                if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIAction))
                {
                    return true;
                }
            }

            return false;
        }
        
        private bool TryTakeEnemyAIAction(Unit enemyUnit,Action onEnemyAIAction)
        {
            var spinActon = enemyUnit.GetSpinAction();
            
            var actionGridPosition = enemyUnit.GetGridPosition();
            
            if (spinActon == null) return false;
                
            if (!spinActon.IsValidActionGridPositions(actionGridPosition)) return false;
                
            if (!enemyUnit.TrySpendActionPointToTakeAction(spinActon)) return false;
            
            spinActon.TakeAction(actionGridPosition, onEnemyAIAction);
            return true;
        }

        private void SetStateTakingTurn()
        {
            timer = 0.5f;
            state = State.TakingTurn;
        }
    }

    public enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }
}