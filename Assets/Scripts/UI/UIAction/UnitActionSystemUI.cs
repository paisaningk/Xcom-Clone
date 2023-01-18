using System;
using System.Collections.Generic;
using TMPro;
using Turn;
using UnitClass;
using UnityEngine;

namespace UI.UIAction
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField] private ActionButtonUI actionButtonPrefab;
        [SerializeField] private Transform actionButtonContainerTransform;
        [SerializeField] private TMP_Text actionPointText;

        private List<ActionButtonUI> actionButtonUiList;
        private void Start()
        {
            actionButtonUiList = new List<ActionButtonUI>();
            
            UnitActionSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged += OnSelectedActionChanged;
            UnitActionSystem.Instance.OnActionStarted += OnActionStarted;
            TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
            Unit.OnAnyActionPointsChanged += OnAnyActionPointsChanged;
            
            CreateUnitActionButtons();
            
            UpdateSelectedVisual();

            UpdateActionPoints();
        }

        private void CreateUnitActionButtons()
        {
            //เอาตัวลูกมาทำลาย ท่าใหม่
            var selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

            if (selectedUnit != null)
            {
                foreach (Transform variable in actionButtonContainerTransform)
                {
                    Destroy(variable.gameObject);
                }
                
                actionButtonUiList.Clear();

                foreach (var baseAction in selectedUnit.GetBaseActionArray())
                {
                    var instantiate = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
                    instantiate.SetBaseAction(baseAction);
                    
                    actionButtonUiList.Add(instantiate);
                }
            }
        }

        private void OnSelectedUnitChanged(object sender, EventArgs eventArgs)
        {
            CreateUnitActionButtons();
            UpdateSelectedVisual();
            UpdateActionPoints();
        }

        private void OnSelectedActionChanged(object sender, EventArgs eventArgs)
        {
            UpdateSelectedVisual();
        }

        private void OnTurnChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }

        public void UpdateSelectedVisual()
        {
            foreach (var actionButtonUI in actionButtonUiList)
            {
                actionButtonUI.UpdateSelectedVisual();
            }
        }

        private void OnAnyActionPointsChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }

        private void OnActionStarted(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }

        private void UpdateActionPoints()
        {
            var selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

            if (selectedUnit != null)
            {
                actionPointText.text = $"Action Point : {selectedUnit.GetActionPoint()}";
            }
        }
    }
}