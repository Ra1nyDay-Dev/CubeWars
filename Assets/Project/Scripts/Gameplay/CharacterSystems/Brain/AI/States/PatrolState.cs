using Project.Scripts.Infrastructure.StateMachine;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.States
{
     public class PatrolState : IState
    {
        private const int MaxPickAttempts = 4;
 
        private readonly BotContext _ctx;
 
        private float _idleTimer;
        private bool _isIdling;
 
        private Vector3 _lastStuckCheckPosition;
        private float _stuckCheckTimer;
 
        public PatrolState(BotContext ctx) =>
            _ctx = ctx;
 
        public void Enter()
        {
            _isIdling = false;
            _idleTimer = 0f;
            PickNewDestination();
        }
 
        public void Exit()
        {
            _ctx.Character.Movement.SetMoveDirection(Vector2.zero);
            _ctx.AgentMovement.Stop();
        }
 
        public void Tick(float deltaTime)
        {
            if (_isIdling)
            {
                _idleTimer -= deltaTime;
                if (_idleTimer <= 0f)
                {
                    _isIdling = false;
                    PickNewDestination();
                }
                return;
            }
 
            Vector3 dir = _ctx.AgentMovement.Tick();
 
            if (_ctx.AgentMovement.Arrived(_ctx.Config.PatrolArriveDistance))
            {
                StartIdle();
                return;
            }
 
            if (dir.sqrMagnitude > 0.0001f)
            {
                _ctx.Character.Movement.SetMoveDirection(new Vector2(dir.x, dir.z));
                _ctx.Character.Movement.SetRotationDirection(dir);
            }
 
            UpdateStuckCheck(deltaTime);
        }
 
        private void UpdateStuckCheck(float deltaTime)
        {
            _stuckCheckTimer += deltaTime;
            if (_stuckCheckTimer < _ctx.Config.StuckCheckInterval)
                return;
 
            Vector3 currentPos = _ctx.Character.transform.position;
            float threshold = _ctx.Config.StuckDistanceThreshold;
 
            if ((currentPos - _lastStuckCheckPosition).sqrMagnitude < threshold * threshold)
            {
                PickNewDestination();
                return;
            }
 
            _lastStuckCheckPosition = currentPos;
            _stuckCheckTimer = 0f;
        }
 
        private void StartIdle()
        {
            _isIdling = true;
            _idleTimer = _ctx.Config.PatrolIdleTime;
            _ctx.Character.Movement.SetMoveDirection(Vector2.zero);
        }
 
        private void PickNewDestination()
        {
            ResetStuckTracking();
 
            Vector3 forward = GetForwardHint();
 
            for (int i = 0; i < MaxPickAttempts; i++)
            {
                if (_ctx.PatrolPointProvider.TryGetNextPoint(forward, out Vector3 point)
                    && _ctx.AgentMovement.SetDestination(point))
                    return;
            }
 
            StartIdle();
        }
 
        private Vector3 GetForwardHint()
        {
            Vector2 vel = _ctx.Character.Movement.CurrentHorizontalVelocity;
            Vector3 v3 = new(vel.x, 0f, vel.y);
            if (v3.sqrMagnitude > 0.01f)
                return v3;
            return _ctx.Character.transform.forward;
        }
 
        private void ResetStuckTracking()
        {
            _lastStuckCheckPosition = _ctx.Character.transform.position;
            _stuckCheckTimer = 0f;
        }
    }
}