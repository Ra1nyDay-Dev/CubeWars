using System;
using System.Collections.Generic;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.Brain;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI;
using Project.Scripts.Gameplay.Services.CameraProvider;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using Project.Scripts.Infrastructure.Services.Input;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.Services.Factories.BrainFactory
{
    public class BrainFactory : IBrainFactory, ITickable, IDisposable
    {
        public PlayerCharacterBrain PlayerBrain {get; private set;}
        public List<CharacterBrain> Brains => new(_brains);

        private readonly List<CharacterBrain> _brains;
        
        private readonly IInputService _inputService;
        private readonly ICameraProvider _cameraProvider;
        private readonly IConfigProvider _configProvider;

        [Inject]
        public BrainFactory(
            IInputService inputService,
            ICameraProvider cameraProvider,
            IConfigProvider configProvider)
        {
            _inputService = inputService;
            _cameraProvider = cameraProvider;
            _configProvider = configProvider;
            
            _brains = new List<CharacterBrain>();
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
                    new AiBrain(character, _configProvider.GetAiBotConfig()),
                _ => 
                    new EmptyCharacterBrain(character),
            };
            
            _brains.Add(characterBrain);
            characterBrain.Enable();
            
            if (characterBrain is PlayerCharacterBrain playerCharacterBrain)
                PlayerBrain = playerCharacterBrain;
            
            return characterBrain;
        }
        
        public void Tick()
        {
            float delta = Time.deltaTime;

            foreach (var brain in _brains)
                brain.Tick();
        }
        
        public void Dispose()
        {
            foreach (CharacterBrain brain in _brains) 
                brain.Dispose();
            
            _brains.Clear();
            PlayerBrain = null;
        }
    }
}