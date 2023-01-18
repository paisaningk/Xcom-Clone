using System;
using UnityEngine;

namespace UnitClass
{
    public class UnitSelectedVisual : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        private MeshRenderer meshRenderer;

        private void OnValidate()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            UpdateVisual();
        }

        private void OnEnable()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
        }

        private void OnDestroy()
        {
            if (UnitActionSystem.Instance != null)
            {
                UnitActionSystem.Instance.OnSelectedUnitChanged -= OnSelectedUnitChanged;
            }
        }

        private void OnSelectedUnitChanged(object sender, EventArgs eventArgs)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            meshRenderer.enabled = (UnitActionSystem.Instance.GetSelectedUnit() == unit);
        }
    }
}
