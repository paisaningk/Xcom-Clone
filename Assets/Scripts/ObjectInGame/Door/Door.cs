using System;
using System.Collections;
using AStar;
using Grid;
using InterfaceClass;
using UnityEngine;

namespace ObjectInGame.Door
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool isOpen;
        [SerializeField] private Animator animator;
        private GridPosition gridPosition;
        private readonly int isOpenHash = Animator.StringToHash("isOpen");
        private float timer;
        private Action onInteractComplete;

        public void Start()
        {
            gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

            if (isOpen)
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }
        }

        public void Interact(Action onInteractionComplete)
        {
            this.onInteractComplete = onInteractionComplete;
            timer = 0.5f;
            if (isOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }

            StartCoroutine(CoolDownTimer());
        }

        IEnumerator CoolDownTimer()
        {
            yield return new WaitForSeconds(timer);
            onInteractComplete();
        }

        private void OpenDoor()
        {
            isOpen = true;
            PathFinding.Instance.SetWalkableGridPosition(gridPosition, true);
            animator.SetBool(isOpenHash, isOpen);
        }

        private void CloseDoor()
        {
            isOpen = false;
            PathFinding.Instance.SetWalkableGridPosition(gridPosition, false);
            animator.SetBool(isOpenHash, isOpen);
        }
    }
}