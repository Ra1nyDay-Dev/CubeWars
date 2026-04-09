using Project.Scripts.Infrastructure.Services.Input;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.Brain
{
    public class PlayerCharacterBrain : CharacterBrain
    {
        private readonly IInputService _inputService;
        
        public PlayerCharacterBrain(GameObject character, IInputService inputService) : base(character) => 
            _inputService = inputService;

        protected override void UpdateLogic(float deltaTime) => 
            HandleInput();

        private void HandleInput()
        {
            GameplayInput input = _inputService.GetGameplayInput(_character.transform.position);
            
            SetMoveDirection(input.Move);
            SetRotationDirection(input.Look);

            if (input.Jump)
                Jump();
             
            if (input.Interact)
                TryInteract();
            
            if (input.StartPrimaryFire)
                StartPrimaryAttack();
            
            if (input.StopPrimaryFire)
                StopPrimaryAttack();
            
            if (input.StartSecondaryFire)
                StartSecondaryAttack();
            
            if (input.StopSecondaryFire)
                StopSecondaryAttack();
            
            if (input.Reload) 
                Reload();
        }
    }
}