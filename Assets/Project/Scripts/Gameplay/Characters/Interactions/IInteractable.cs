using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.Interactions
{
    public interface IInteractable
    {
        void Interact(InteractorUnit interactor);
    }
}