using Project.Scripts.Infrastructure.Services.Input;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain
{
    public class BrainsTest : MonoBehaviour
    {
        // [SerializeField] private Character _character;
        //
        // private CharacterBrain _characterBrain;
        // private IInputService _inputService;
        // private Camera _camera;
        //
        // [Inject]
        // private void Construct(IInputService inputService) => 
        //     _inputService = inputService;
        //
        // private void Start()
        // {
        //     _camera = Camera.main;
        //     _characterBrain = new PlayerCharacterBrain(_character, _camera, _inputService);
        //     _characterBrain.Enable();
        // }
        //
        // private void Update()
        // {
        //     _characterBrain.Tick();
        // }
        //
        // private void OnDestroy() => 
        //     _characterBrain.Dispose();
    }
}