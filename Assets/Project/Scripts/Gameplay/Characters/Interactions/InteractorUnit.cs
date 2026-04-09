using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.Interactions
{
    public class InteractorUnit : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _interactZoneTriggerObserver;
        
        public IInteractable CurrentInteractable { get; private set; }
        
        private void Start()
        {
            _interactZoneTriggerObserver.TriggerEnter += TriggerEnter;
            _interactZoneTriggerObserver.TriggerExit += TriggerExit;
        }

        private void TriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable)) 
                CurrentInteractable = interactable;
        }

        private void TriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable) && 
                CurrentInteractable == interactable)
                CurrentInteractable = null;
        }

        private void OnDestroy()
        {
            CurrentInteractable = null;
            _interactZoneTriggerObserver.TriggerEnter -= TriggerEnter;
            _interactZoneTriggerObserver.TriggerExit -= TriggerExit;
        }
    }
}