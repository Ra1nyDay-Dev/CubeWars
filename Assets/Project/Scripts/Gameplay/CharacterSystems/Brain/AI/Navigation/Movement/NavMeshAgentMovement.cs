using Project.Scripts.Gameplay.CharacterSystems.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshAgentMovement : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private CharacterMovement _characterMovement;
        private float _arriveDistance = 1f;

        public void Construct(float arriveDistance) => 
            _arriveDistance = arriveDistance;

        public void Initialize()
        {
            _agent = GetComponent<NavMeshAgent>();
            _characterMovement = GetComponent<CharacterMovement>();

            if (_characterMovement != null)
            {
                _agent.updatePosition = false;
                _agent.updateRotation = false;
                _agent.updateUpAxis = false;
                
                _agent.speed = _characterMovement.MovementSpeed;
                _agent.acceleration = _characterMovement.Acceleration;
                _agent.angularSpeed = _characterMovement.MovementSpeed;
            }
 
            Warp(transform.position);
        }
 
        public bool HasPath => 
            _agent.hasPath && !_agent.pathPending;
        
        public float RemainingDistance => 
            _agent.hasPath ? _agent.remainingDistance : 0;
 
        public bool SetDestination(Vector3 destination) =>
            _agent.SetDestination(destination);
 
        public void Stop() =>
            _agent.ResetPath();
 
        public void Warp(Vector3 position) =>
            _agent.Warp(position);
        
        public Vector3 GetCurrentMoveDirection()
        {
            _agent.nextPosition = transform.position;
 
            if (!HasPath)
                return Vector3.zero;
 
            Vector3 direction = _agent.desiredVelocity;
            direction.y = 0f;
            return direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector3.zero;
        }
 
        public bool Arrived()
        {
            if (!HasPath)
                return false;
 
            return _agent.remainingDistance <= _arriveDistance;
        }
    }
}