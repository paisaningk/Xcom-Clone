using System;
using Turn;
using UnityEngine;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float setTimer = 2f;
        private float timer;

        public void Start()
        {
            TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
        }

        public void Update()
        {
            if (TurnSystem.Instance.IsPlayerTurn()) return;

            timer -= Time.deltaTime;
            
            if (timer <= 0)
            {
                TurnSystem.Instance.NextTurn();
            }
        }

        private void OnTurnChanged(object sender, EventArgs e)
        {
            timer = setTimer;
        }
    }
}