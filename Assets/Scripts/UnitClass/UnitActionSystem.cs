using System;
using Grid;
using Turn;
using UnitAction;
using UnityEngine;
using Utils;
using UnityEngine.EventSystems;

namespace UnitClass
{
    public sealed class UnitActionSystem : Singleton<UnitActionSystem>
    {
        public event EventHandler OnSelectedUnitChanged;
        public event EventHandler OnSelectedActionChanged;
        public event EventHandler<bool> OnBusyChanged;
        public event EventHandler OnActionStarted;
        
        [SerializeField] private Unit selectedUnit;
        [SerializeField] private LayerMask unitLayerMask;

        private BaseAction selectedAction;
        private bool isBusy;
    
        public void Update()
        {
            if (!TurnSystem.Instance.IsPlayerTurn()) return;
            
            if (isBusy) return;
            
            //check mouse is pointer on ui
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            if (TryHandleUnitSelection()) return;

            HandleSelectedAction();
        }

        private void HandleSelectedAction()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var mouseGridPosition = LevelGrid.Instance.GetGridPosition(MousePosition.Instance.GetPointPosition());
                
                if (selectedAction == null) return;
                
                if (!selectedAction.IsValidActionGridPositions(mouseGridPosition)) return;
                
                if (!selectedUnit.TrySpendActionPointToTakeAction(selectedAction)) return;
                
                SetBusy();

                OnOnActionStarted();
                
                selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }
        }
        

        private bool TryHandleUnitSelection()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var raycast = Raycaster.Instance.UseRaycastByMouse(unitLayerMask);

                if (raycast.isHit)
                {
                    if (raycast.hitInfo.collider.TryGetComponent(out Unit unit))
                    {
                        if (selectedUnit == unit)
                        {
                            return false;
                        }

                        if (unit.IsEnemy())
                        {
                            return false;
                        }
                        
                        SelectUnit(unit);
                        return true;
                    }
                }
            }

            return false;
        }

        private void SelectUnit(Unit unit)
        {
            selectedUnit = unit;
            SetSelectedAction(unit.GetAction<MoveAction>());
            
            OnOnSelectedUnitChanged();
        }

        public void SetSelectedAction(BaseAction baseAction)
        {
            selectedAction = baseAction;
            
            OnOnSelectedActionChanged();
        }

        public void SetBusy()
        {
            isBusy = true;
            
            OnBusyChanged?.Invoke(this, isBusy);
        }

        public void ClearBusy()
        {
            isBusy = false;
            
            OnBusyChanged?.Invoke(this, isBusy);
        }

        public Unit GetSelectedUnit()
        {
            return selectedUnit;
        }

        public BaseAction GetSelectAction()
        {
            return selectedAction;
        }

        private void OnOnSelectedUnitChanged()
        {
            OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnOnSelectedActionChanged()
        {
            OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnOnActionStarted()
        {
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }
}
