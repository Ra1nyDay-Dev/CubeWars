using Project.Scripts.Gameplay.Data.Configs.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation
{
    public class NavMeshAgentMovement
    {
        private readonly Transform _transform;
        private readonly NavMeshAgent _agent;
 
        public NavMeshAgentMovement(Character character, AiBotConfig config)
        {
            _transform = character.transform;
            _agent = character.gameObject.GetComponent<NavMeshAgent>();
            
            // controls by characterMovement
            _agent.updatePosition = false;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            
            // implemented just for desiredVelocity calculation.
            _agent.speed = character.Movement.MovementSpeed;
            _agent.acceleration = character.Movement.Acceleration;
            _agent.angularSpeed = character.Movement.MovementSpeed;
 
            _agent.Warp(_transform.position);
        }
 
        public bool HasPath => 
            _agent.hasPath && !_agent.pathPending;
        
        public float RemainingDistance => 
            _agent.hasPath ? _agent.remainingDistance : float.PositiveInfinity;
 
        public bool SetDestination(Vector3 destination) =>
            _agent.SetDestination(destination);
 
        public void Stop() =>
            _agent.ResetPath();
 
        public void Warp(Vector3 position) =>
            _agent.Warp(position);
        
        public Vector3 Tick()
        {
            _agent.nextPosition = _transform.position;
 
            if (!HasPath)
                return Vector3.zero;
 
            Vector3 dir = _agent.desiredVelocity;
            dir.y = 0f;
            return dir.sqrMagnitude > 0.0001f ? dir.normalized : Vector3.zero;
        }
 
        public bool Arrived(float arriveDistance)
        {
            if (!HasPath)
                return false;
 
            return _agent.remainingDistance <= arriveDistance;
        }
    }
}