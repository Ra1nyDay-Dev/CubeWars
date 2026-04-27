using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Combat;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.Movement;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Sensors;
using Project.Scripts.Gameplay.Data.Configs.AI;
using Project.Scripts.Gameplay.WeaponSystems;
using Project.Scripts.Infrastructure.StateMachine;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.States
{
    public class ChaseEnemyState : IState
    {
        private readonly Character _character;
        private readonly NavMeshAgentMovement _agentMovement;
        private readonly StuckDetector _stuckDetector;
        private readonly EnemySensor _enemySensor;
        private readonly AttackPositionProvider _positionProvider;
        private readonly BotConfig _botConfig;
 
        private Character _target;
        private float _repositionTimer;
 
        public ChaseEnemyState(
            Character character,
            NavMeshAgentMovement agentMovement,
            StuckDetector stuckDetector,
            EnemySensor enemySensor,
            AttackPositionProvider positionProvider,
            BotConfig botConfig)
        {
            _character = character;
            _agentMovement = agentMovement;
            _stuckDetector = stuckDetector;
            _enemySensor = enemySensor;
            _positionProvider = positionProvider;
            _botConfig = botConfig;
        }
 
        public void Enter()
        {
            _target = _enemySensor.FindNearestVisibleEnemy();
            _stuckDetector.ResetTimer();
            PickAttackPoint();
        }
 
        public void Exit()
        {
            _target = null;
            _character.Movement.SetMoveDirection(Vector2.zero);
            _agentMovement.Stop();
        }
 
        public void Tick(float deltaTime)
        {
            if (!_enemySensor.IsVisible(_target))
            {
                _target = _enemySensor.FindNearestVisibleEnemy();
                if (_target == null)
                {
                    _character.Movement.SetMoveDirection(Vector2.zero);
                    return;
                }
 
                PickAttackPoint();
            }
 
            _repositionTimer -= deltaTime;
            if (_repositionTimer <= 0f || _agentMovement.Arrived())
                PickAttackPoint();
 
            Vector3 moveDirection = _agentMovement.GetCurrentMoveDirection();
            if (moveDirection.sqrMagnitude > 0.0001f)
            {
                _character.Movement.SetMoveDirection(new Vector2(moveDirection.x, moveDirection.z));
                _character.Movement.SetRotationDirection(moveDirection);
            }
            else
                _character.Movement.SetMoveDirection(Vector2.zero);

            if (_stuckDetector.IsStuckInThisFrame(deltaTime))
                PickAttackPoint();
        }
 
        private void PickAttackPoint()
        {
            _stuckDetector.ResetTimer();
            _repositionTimer = _botConfig.CombatRepositionInterval;
 
            if (_target == null)
                return;
 
            IWeapon weapon = _character.WeaponArsenal.CurrentWeapon;
            if (weapon == null)
                return;
 
            (float min, float max) = CombatRangeResolver.GetApproachRadius(weapon, _botConfig);
            if (_positionProvider.TryGetRandomPointAround(_target.transform.position, min, max, out Vector3 point))
                _agentMovement.SetDestination(point);
        }
    }
}