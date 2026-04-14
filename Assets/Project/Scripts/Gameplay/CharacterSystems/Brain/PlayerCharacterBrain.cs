using Project.Scripts.Infrastructure.Services.Input;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain
{
    public class PlayerCharacterBrain : CharacterBrain
    {
        private readonly IInputService _inputService;
        private readonly Camera _camera;

        public PlayerCharacterBrain(
            Character character,
            Camera camera,
            IInputService inputService
        ) : base(character)
        {
            _inputService = inputService;
            _camera = camera;

            SubscribeInputActions();
        }

        public override void Dispose()
        {
            base.Dispose();
            UnsubscribeInputActions();
        }

        protected override void UpdateLogic(float deltaTime) => 
            HandleInput();

        private void HandleInput()
        {
            Move(_inputService.GameplayActions.GetRelativeMoveInput(_camera));
            
            Rotate(_inputService.GameplayActions.GetRelativeLookInput(
                    _character.transform.position,
                    _camera
                    )
            );
        }

        private void SubscribeInputActions()
        {
            _inputService.GameplayActions.PrimaryButtonDown += StartPrimaryAttack;
            _inputService.GameplayActions.PrimaryButtonUp += StopPrimaryAttack;
            _inputService.GameplayActions.SecondaryButtonDown += StartSecondaryAttack;
            _inputService.GameplayActions.SecondaryButtonUp += StopSecondaryAttack;
            _inputService.GameplayActions.ReloadButtonDown += Reload;
            _inputService.GameplayActions.InteractButtonDown += TryInteract;
            _inputService.GameplayActions.JumpButtonDown += Jump;
        }
        
        private void UnsubscribeInputActions()
        {
            _inputService.GameplayActions.PrimaryButtonDown -= StartPrimaryAttack;
            _inputService.GameplayActions.PrimaryButtonUp -= StopPrimaryAttack;
            _inputService.GameplayActions.SecondaryButtonDown -= StartSecondaryAttack;
            _inputService.GameplayActions.SecondaryButtonUp -= StopSecondaryAttack;
            _inputService.GameplayActions.ReloadButtonDown -= Reload;
            _inputService.GameplayActions.InteractButtonDown -= TryInteract;
            _inputService.GameplayActions.JumpButtonDown -= Jump;
        }
    }
}