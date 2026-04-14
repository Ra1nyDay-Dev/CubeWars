using System;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.Brain;
using Project.Scripts.Gameplay.Services.BrainsHolder;
using Project.Scripts.Gameplay.Services.CameraProvider;
using Project.Scripts.Infrastructure.Services.Input;
using Zenject;

namespace Project.Scripts.Gameplay.Services.Factories.BrainFactory
{
    public class BrainFactory : IBrainFactory
    {
        private readonly IInputService _inputService;
        private readonly ICameraProvider _cameraProvider;
        private readonly IBrainsHolder _brainsHolder;

        [Inject]
        public BrainFactory(
            IInputService inputService,
            ICameraProvider cameraProvider,
            IBrainsHolder brainsHolder)
        {
            _inputService = inputService;
            _cameraProvider = cameraProvider;
            _brainsHolder = brainsHolder;
        }
        
        public CharacterBrain Create(Character character, BrainType brainType)
        {
            CharacterBrain characterBrain = brainType switch
            {
                BrainType.Player => 
                    new PlayerCharacterBrain(
                        character,
                        _cameraProvider.Camera,
                        _inputService),
                BrainType.Ai => 
                    new AiBrain(character),
                _ => 
                    new EmptyCharacterBrain(character),
            };
            
            _brainsHolder.Add(characterBrain);
            characterBrain.Enable();
            
            return characterBrain;
        }
    }
}