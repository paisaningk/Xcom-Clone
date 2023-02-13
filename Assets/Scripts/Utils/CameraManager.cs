using System;
using UnitAction;
using UnitClass;
using Unity.Mathematics;
using UnityEngine;

namespace Utils
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameObject actionCameraGameObject;
        [SerializeField] private float shoulderOffsetAmount = 0.5f;

        private void Start()
        {
            HideActionCamera();
            BaseAction.OnAnyActionStart += BaseAction_OnAnyActionStart;
            BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
        }

        private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
        {
            switch (sender)
            {
                case ShootAction:
                    HideActionCamera();
                    break;
            }
        }

        private void BaseAction_OnAnyActionStart(object sender, EventArgs e)
        {
            switch (sender)
            {
                case ShootAction shootAction:
                    var shooterUnit = shootAction.GetUnit();
                    var targetUnit = shootAction.GetTargetUnit();
                    
                    var cameraCharacterHeight = Vector3.up * 1.7f;

                    var shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                    var shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;

                    var shootUnitHeightPos = shooterUnit.GetWorldPosition() + cameraCharacterHeight;

                    var shootUnitShouldOffset = shootUnitHeightPos + shoulderOffset;
                    
                    // * -1 for backward
                    var actionCameraPosition = shootUnitShouldOffset + (shootDir * -1);

                    actionCameraGameObject.transform.position = actionCameraPosition;
                    actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                    ShowActionCamera();
                    break;
            }
        }

        private void ShowActionCamera()
        {
            actionCameraGameObject.SetActive(true);
        }

        private void HideActionCamera()
        {
            actionCameraGameObject.SetActive(false);
        }
    }
}