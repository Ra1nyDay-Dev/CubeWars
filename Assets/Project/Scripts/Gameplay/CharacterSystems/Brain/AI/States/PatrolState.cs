using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.Movement;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.PointProvider;
using Project.Scripts.Gameplay.CharacterSystems.Movement;
using Project.Scripts.Infrastructure.StateMachine;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.States
{
     public class PatrolState : IState
    {
        private const int MAX_PICK_ATTEMPTS = 10;
        
        private readonly CharacterMovement _characterMovement;
        private readonly NavMeshAgentMovement _agentMovement;
        private readonly StuckDetector _stuckDetector;
        private readonly IPatrolPointProvider _patrolPointProvider;

        public PatrolState(
            CharacterMovement characterMovement,
            NavMeshAgentMovement agentMovement,
            StuckDetector stuckDetector,
            IPatrolPointProvider patrolPointProvider)
        {
            _characterMovement = characterMovement;
            _agentMovement = agentMovement;
            _stuckDetector =  stuckDetector;
            _patrolPointProvider = patrolPointProvider;
        }

        public void Enter() => 
            PickNewDestination();

        public void Exit()
        {
            _characterMovement.SetMoveDirection(Vector2.zero);
            _agentMovement.Stop();
        }
 
        public void Tick(float deltaTime)
        {
            if (_agentMovement.Arrived())
                PickNewDestination();
            
            Vector3 direction = _agentMovement.GetCurrentMoveDirection();
            _characterMovement.SetMoveDirection(new Vector2(direction.x, direction.z));
            _characterMovement.SetRotationDirection(direction);
            
            if (_stuckDetector.IsStuckInThisFrame(deltaTime))
                PickNewDestination();
        }
 
        private void PickNewDestination()
        {
            _stuckDetector.ResetTimer();
 
            for (int i = 0; i < MAX_PICK_ATTEMPTS; i++)
            {
                if (_patrolPointProvider.TryGetNextPoint(GetMoveDirection(), out Vector3 point)
                    && _agentMovement.SetDestination(point))
                    return;
            }
 
            _characterMovement.SetMoveDirection(Vector2.zero);
        }
 
        private Vector3 GetMoveDirection()
        {
            Vector2 velocity = _characterMovement.CurrentHorizontalVelocity;
            Vector3 velocityVector3 = new(velocity.x, 0f, velocity.y);
            
            if (velocityVector3.sqrMagnitude > 0.01f)
                return velocityVector3;
            
            return _characterMovement.transform.forward;
        }
        
    }
}