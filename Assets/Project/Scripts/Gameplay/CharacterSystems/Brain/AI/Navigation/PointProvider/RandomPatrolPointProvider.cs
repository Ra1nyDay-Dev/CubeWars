using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.PointProvider
{
    public class RandomPatrolPointProvider : IPatrolPointProvider
    {
        private readonly Transform _agentTransform;
        private readonly float _pointSearchRadius;
        private readonly float _sampleDistance;
        private readonly int _maxAttempts;
 
        public RandomPatrolPointProvider(Transform agentTransform, float pointSearchRadius, float sampleDistance = 2f, int maxAttempts = 10)
        {
            _agentTransform = agentTransform;
            _pointSearchRadius = pointSearchRadius;
            _sampleDistance = sampleDistance;
            _maxAttempts = maxAttempts;
        }
 
        public bool TryGetNextPoint(Vector3 moveDirection, out Vector3 point)
        {
            for (int i = 0; i < _maxAttempts; i++)
            {
                Vector2 circle = Random.insideUnitCircle * _pointSearchRadius;
                Vector3 candidate = _agentTransform.position + new Vector3(circle.x, 0f, circle.y);
 
                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, _sampleDistance, NavMesh.AllAreas))
                {
                    point = hit.position;
                    return true;
                }
            }
 
            point = _agentTransform.position;
            return false;
        }
    }
}