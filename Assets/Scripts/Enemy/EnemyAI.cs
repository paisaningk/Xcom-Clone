using System;
using Grid;
using Turn;
using UnitAction;
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
            EnemyAIAction bestEnemyAIAction = null;
            BaseAction bestBaseAction = null;
            
            foreach (var baseAction in enemyUnit.GetBaseActionArray())
            {
                if (!enemyUnit.CanSpendActionPointToTakeAction(baseAction))
                {
                    continue;
                }

                if (bestEnemyAIAction == null)
                {
                    bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    bestBaseAction = baseAction;
                }
                else
                {
                    var testEnemyAiAction = baseAction.GetBestEnemyAIAction();
                    if (testEnemyAiAction != null && testEnemyAiAction.actionValue > bestEnemyAIAction.actionValue)
                    {
                        bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                        bestBaseAction = baseAction;
                    }
                }
                
            }

            if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointToTakeAction(bestBaseAction))
            {
                bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIAction);
                return true;
            }

            return false;
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