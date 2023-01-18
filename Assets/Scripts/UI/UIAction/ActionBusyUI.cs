using System;
using UnitClass;
using UnityEngine;

namespace UI
{
    public class ActionBusyUI : MonoBehaviour
    {
        [SerializeField] private GameObject actionBusyGameObject;

        private void Start()
        {
            UnitActionSystem.Instance.OnBusyChanged += OnBusyChanged;
            Hide();
        }

        private void OnBusyChanged(object sender, bool isBusy)
        {
            if (isBusy)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Show()
        {
            actionBusyGameObject.SetActive(true);
        }

        public void Hide()
        {
            actionBusyGameObject.SetActive(false);
        }
    }
}
