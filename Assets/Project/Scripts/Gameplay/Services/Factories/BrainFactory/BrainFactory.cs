using System;
using System.Collections.Generic;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.Brain;
using Project.Scripts.Gameplay.Services.CameraProvider;
using Project.Scripts.Infrastructure.Services.Input;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.Services.Factories.BrainFactory
{
    public class BrainFactory : IBrainFactory, ITickable, IDisposable
    {
        public List<CharacterBrain> Brains => new(_brains);

        private readonly List<CharacterBrain> _brains;
        
        private readonly IInputService _inputService;
        private readonly ICameraProvider _cameraProvider;

        [Inject]
        public BrainFactory(
            IInputService inputService,
            ICameraProvider cameraProvider)
        {
            _inputService = inputService;
            _cameraProvider = cameraProvider;
            
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
                    new AiBrain(character),
                _ => 
                    new EmptyCharacterBrain(character),
            };
            
            _brains.Add(characterBrain);
            characterBrain.Enable();
            
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
        }
    }
}