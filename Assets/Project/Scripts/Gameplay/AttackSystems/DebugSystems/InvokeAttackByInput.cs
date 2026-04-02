using UnityEngine;

namespace Project.Scripts.Gameplay.AttackSystems.DebugSystems
{
    public class InvokeAttackByInput : MonoBehaviour
    {
        [SerializeField] private AttackBehaviour _attackBehaviour;

        private const KeyCode LEFT_MOUSE_BUTTON = KeyCode.Mouse0;

        private void Update()
        {
            if (Input.GetKeyDown(LEFT_MOUSE_BUTTON)) 
                _attackBehaviour.PerformAttack();
        }
    }
}