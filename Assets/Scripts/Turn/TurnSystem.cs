using System;
using UnityEngine;
using Utils;

namespace Turn
{
    public class TurnSystem : Singleton<TurnSystem>
    {
        public event EventHandler OnTurnChanged;
        private int turnNumber = 1;
        private bool isPlayerTurn = true;

        public void NextTurn()
        {
            turnNumber++;
            isPlayerTurn = !isPlayerTurn;
            OnTurnChangedInvoke();
        }

        public int GetTurnNumber()
        {
            return turnNumber;
        }

        public bool IsPlayerTurn()
        {
            return isPlayerTurn;
        }

        public void OnTurnChangedInvoke()
        {
            OnTurnChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
