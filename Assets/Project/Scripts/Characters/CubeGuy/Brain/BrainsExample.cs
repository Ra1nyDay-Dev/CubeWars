using UnityEngine;

namespace Project.Scripts.Characters.CubeGuy.Brain
{
    public class BrainsExample : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private Camera _camera;
        
        private CharacterBrain _characterBrain;

        private void Awake()
        {
            _characterBrain = new PlayerCharacterBrain(_character, _camera);
            _characterBrain.Enable();
        }

        private void Update()
        {
            _characterBrain.Update(Time.deltaTime);
        }
    }
}