using System;
using TMPro;
using Turn;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TurnSystemUI : MonoBehaviour
    {
        [SerializeField] private Button endTurnButton;
        [SerializeField] private TMP_Text turnText;
        [SerializeField] private GameObject enemyTurnVisualGameObject;

        private void Start()
        {
            UpdateTurnText();
            endTurnButton.onClick.AddListener( () => TurnSystem.Instance.NextTurn());

            TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
            UpdateEndTurnButtonVisibility();
        }

        private void OnTurnChanged(object sender, EventArgs e)
        {
            UpdateTurnText();
            SetEnemyTurnVisualGameObject();
            UpdateEndTurnButtonVisibility();
        }

        private void UpdateTurnText()
        {
            turnText.text = $"Turn : {TurnSystem.Instance.GetTurnNumber()}";
        }

        private void SetEnemyTurnVisualGameObject()
        {
            enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
        }

        private void UpdateEndTurnButtonVisibility()
        {
            endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
        }
    }
}
