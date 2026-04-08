using Project.Scripts.Gameplay.Characters.Movement;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.Brain
{
    public class BrainsTest : MonoBehaviour
    {
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private Camera _camera;
        
        private CharacterBrain _characterBrain;

        private void Awake()
        {
            _characterBrain = new PlayerCharacterBrain(_characterMovement, _camera);
            _characterBrain.Enable();
        }

        private void Update()
        {
            _characterBrain.Update(Time.deltaTime);
        }
    }
}