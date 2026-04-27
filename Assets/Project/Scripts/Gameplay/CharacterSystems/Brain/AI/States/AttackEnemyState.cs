using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Combat;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.Movement;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Sensors;
using Project.Scripts.Gameplay.CharacterSystems.Inventory;
using Project.Scripts.Gameplay.Data.Configs.AI;
using Project.Scripts.Gameplay.WeaponSystems;
using Project.Scripts.Infrastructure.StateMachine;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.States
{
    public class AttackEnemyState : IState
    {
        private readonly Character _character;
        private readonly NavMeshAgentMovement _agentMovement;
        private readonly StuckDetector _stuckDetector;
        private readonly EnemySensor _enemySensor;
        private readonly AttackPositionProvider _positionProvider;
        private readonly BotConfig _botConfig;
        private readonly WeaponArsenal _arsenal;
 
        private Character _target;
        private IWeapon _trackedWeapon;
        private bool _isAttacking;
        private float _repositionTimer;
 
        public AttackEnemyState(
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
            _arsenal = character.WeaponArsenal;
        }
 
        public void Enter()
        {
            _target = _enemySensor.FindNearestVisibleEnemy();
            _stuckDetector.ResetTimer();
            _arsenal.WeaponChanged += OnWeaponChanged;
            SubscribeToCurrentWeapon();
            PickStrafePoint();
        }
 
        public void Exit()
        {
            StopAttack();
            _arsenal.WeaponChanged -= OnWeaponChanged;
            UnsubscribeFromCurrentWeapon();
            _agentMovement.Stop();
            _character.Movement.SetMoveDirection(Vector2.zero);
            _target = null;
        }
 
        public void Tick(float deltaTime)
        {
            if (!_enemySensor.IsVisible(_target))
            {
                _target = _enemySensor.FindNearestVisibleEnemy();
                if (_target == null)
                {
                    StopAttack();
                    _character.Movement.SetMoveDirection(Vector2.zero);
                    return;
                }
 
                PickStrafePoint();
            }
 
            _repositionTimer -= deltaTime;
            if (_repositionTimer <= 0f || _agentMovement.Arrived())
                PickStrafePoint();
            
            SetMoveDirection();
            SetRotationDirection();

            if (_stuckDetector.IsStuckInThisFrame(deltaTime))
                PickStrafePoint();
            
            if (_enemySensor.HasLineOfSight(_target))
                TryStartAttack();
            else
                StopAttack();
        }

        private void PickStrafePoint()
        {
            _stuckDetector.ResetTimer();
            _repositionTimer = _botConfig.CombatRepositionInterval;
 
            if (_target == null)
                return;
 
            IWeapon weapon = _arsenal.CurrentWeapon;
            if (weapon == null)
                return;
 
            (float min, float max) = CombatRangeResolver.GetApproachRadius(weapon, _botConfig);
            if (_positionProvider.TryGetRandomPointAround(_target.transform.position, min, max, out Vector3 point))
                _agentMovement.SetDestination(point);
        }

        private void TryStartAttack()
        {
            IWeapon weapon = _arsenal.CurrentWeapon;
            if (weapon == null || _isAttacking)
                return;
 
            _isAttacking = true;
            weapon.StartPrimaryAttack().Forget();
        }

        private void StopAttack()
        {
            _isAttacking = false;
            _arsenal.CurrentWeapon?.StopPrimaryAttack();
        }

        private void SetMoveDirection()
        {
            Vector3 moveDirection = _agentMovement.GetCurrentMoveDirection();
            if (moveDirection.sqrMagnitude > 0.0001f)
                _character.Movement.SetMoveDirection(new Vector2(moveDirection.x, moveDirection.z));
            else
                _character.Movement.SetMoveDirection(Vector2.zero);
        }

        private void SetRotationDirection()
        {
            Vector3 rotationToEnemy = _target.transform.position - _character.transform.position;
            rotationToEnemy.y = 0f;
            if (rotationToEnemy.sqrMagnitude > 0.0001f)
                _character.Movement.SetRotationDirection(rotationToEnemy);
        }

        private void OnWeaponChanged(IWeapon weapon)
        {
            UnsubscribeFromCurrentWeapon();
            _isAttacking = false;
            SubscribeToCurrentWeapon();
        }

        private void SubscribeToCurrentWeapon()
        {
            _trackedWeapon = _arsenal.CurrentWeapon;
            if (_trackedWeapon != null)
                _trackedWeapon.PrimaryAttackEnded += OnPrimaryAttackEnded;
        }
 
        private void UnsubscribeFromCurrentWeapon()
        {
            if (_trackedWeapon != null)
                _trackedWeapon.PrimaryAttackEnded -= OnPrimaryAttackEnded;
            _trackedWeapon = null;
        }
 
        private void OnPrimaryAttackEnded()
        {
            _arsenal.CurrentWeapon?.StopPrimaryAttack();
            _isAttacking = false;
        }
    }
}