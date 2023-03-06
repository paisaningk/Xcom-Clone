using System;

namespace InterfaceClass
{
    public interface IInteractable
    {
        public void Interact(Action onInteractionComplete);
    }
}