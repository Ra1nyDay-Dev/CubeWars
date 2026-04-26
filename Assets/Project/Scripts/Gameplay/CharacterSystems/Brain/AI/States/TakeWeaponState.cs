using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.Movement;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Sensors;
using Project.Scripts.Gameplay.CharacterSystems.Interactions;
using Project.Scripts.Gameplay.Data.Configs.AI;
using Project.Scripts.Gameplay.SpawnSystems.WeaponSpawn;
using Project.Scripts.Infrastructure.StateMachine;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.States
{
     public class TakeWeaponState : IState
    {
        private const float RECHECK_INTERVAL = 0.5f;
 
        private readonly Character _character;
        private readonly StuckDetector _stuckDetector;
        private readonly NavMeshAgentMovement _agentMovement;
        private readonly WeaponSpawnerSensor _weaponSpawnerSensor;
        private readonly AiWeaponPriorityConfig _weaponPriorityConfig;
        
        private WeaponSpawner _targetWeaponSpawner;
        private float _recheckTimer;

        public TakeWeaponState(
            Character character,
            NavMeshAgentMovement agentMovement,
            StuckDetector stuckDetector,
            WeaponSpawnerSensor weaponSpawnerSensor,
            AiWeaponPriorityConfig weaponPriorityConfig)
        {
            _character = character;
            _agentMovement = agentMovement;
            _stuckDetector = stuckDetector;
            _weaponSpawnerSensor = weaponSpawnerSensor;
            _weaponPriorityConfig = weaponPriorityConfig;
        }
 
        public void Enter()
        {
            _targetWeaponSpawner = null;
            _recheckTimer = 0f;
            _stuckDetector.ResetTimer();
            AcquireTarget();
        }
 
        public void Exit()
        {
            _targetWeaponSpawner = null;
            _character.Movement.SetMoveDirection(Vector2.zero);
            _agentMovement.Stop();
        }
 
        public void Tick(float deltaTime)
        {
            _recheckTimer -= deltaTime;
 
            if (_targetWeaponSpawner == null || !_targetWeaponSpawner.IsWeaponAvailable || _recheckTimer <= 0f)
            {
                AcquireTarget();
                _recheckTimer = RECHECK_INTERVAL;
            }
 
            if (_targetWeaponSpawner == null)
            {
                _character.Movement.SetMoveDirection(Vector2.zero);
                return;
            }
 
            if (_character.Interactor.TryGetNearInteractable(out IInteractable interactable)
                && ReferenceEquals(interactable, _targetWeaponSpawner))
            {
                _targetWeaponSpawner.Interact(_character.Interactor);
                _targetWeaponSpawner = null;
                return;
            }
 
            Vector3 agentMoveDirection = _agentMovement.GetCurrentMoveDirection();
            if (agentMoveDirection.sqrMagnitude > 0.0001f)
            {
                _character.Movement.SetMoveDirection(new Vector2(agentMoveDirection.x, agentMoveDirection.z));
                _character.Movement.SetRotationDirection(agentMoveDirection);
            }
 
            if (_stuckDetector.IsStuckInThisFrame(deltaTime))
                AcquireTarget();
        }
 
        private void AcquireTarget()
        {
            int currentWeaponPriority = _targetWeaponSpawner != null && _targetWeaponSpawner.IsWeaponAvailable
                ? _weaponPriorityConfig.GetPriority(_targetWeaponSpawner.WeaponType)
                : _weaponPriorityConfig.GetPriority(_character.WeaponArsenal.CurrentWeapon);
 
            WeaponSpawner candidate = _weaponSpawnerSensor.FindBestAvailable(currentWeaponPriority);
            
            if (candidate != null && candidate != _targetWeaponSpawner)
            {
                _targetWeaponSpawner = candidate;
                _agentMovement.SetDestination(_targetWeaponSpawner.transform.position);
                _stuckDetector.ResetTimer();
                return;
            }
 
            if (candidate == null && (_targetWeaponSpawner == null || !_targetWeaponSpawner.IsWeaponAvailable))
            {
                _targetWeaponSpawner = null;
                _agentMovement.Stop();
            }
        }
    }
}