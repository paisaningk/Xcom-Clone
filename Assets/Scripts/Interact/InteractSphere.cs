using System;
using System.Collections;
using Grid;
using InterfaceClass;
using Unity.VisualScripting;
using UnityEngine;

namespace Interact
{
    public class InteractSphere : MonoBehaviour, IInteractable
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Material greenMaterial;
        [SerializeField] private Material redMaterial;
        [SerializeField] private bool isGreen;
        private GridPosition gridPosition;
        private float timer;
        
        private Action onInteractComplete;
        private void Start()
        {
            gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition,this);
            if (isGreen)
            {
                SetColorGreen();
            }
            else
            {
                SetColorRed();
            }
        }

        private void SetColorGreen()
        {
            isGreen = true;
            meshRenderer.material = greenMaterial;
        }

        private void SetColorRed()
        {
            isGreen = false;
            meshRenderer.material = redMaterial; 
        }

        public void Interact(Action onInteractionComplete)
        {
            this.onInteractComplete = onInteractionComplete;

            if (isGreen)
            {
                SetColorRed();
            }
            else
            {
                SetColorGreen();
            }
            
            StartCoroutine(CoolDownTimer());
        }
        
        IEnumerator CoolDownTimer()
        {
            yield return new WaitForSeconds(timer);
            onInteractComplete();
        }
    }
}