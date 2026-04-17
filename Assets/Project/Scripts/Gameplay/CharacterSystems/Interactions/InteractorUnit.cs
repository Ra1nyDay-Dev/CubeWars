using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Interactions
{
    public class InteractorUnit : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _interactZoneTriggerObserver;
        
        private readonly Dictionary<IInteractable, Transform> _interactables = new();

        private void Awake()
        {
            _interactZoneTriggerObserver.TriggerEnter += TriggerEnter;
            _interactZoneTriggerObserver.TriggerExit += TriggerExit;
        }

        private void OnDestroy()
        {
            _interactZoneTriggerObserver.TriggerEnter -= TriggerEnter;
            _interactZoneTriggerObserver.TriggerExit -= TriggerExit;
        }

        public bool TryGetNearInteractable(out IInteractable interactable)
        {
            if (_interactables.Count == 0)
            {
                interactable = null;
                return false;
            }
            
            interactable = _interactables
                .OrderBy(i => Vector3.Distance(transform.position, i.Value.position))
                .First()
                .Key;
            
            return true;
        }

        private void TriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable)) 
                _interactables.Add(interactable, other.transform);
        }

        private void TriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
                _interactables.Remove(interactable);
        }
    }
}