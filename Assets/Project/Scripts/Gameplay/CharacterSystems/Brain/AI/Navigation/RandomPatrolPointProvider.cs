using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation
{
    public class RandomPatrolPointProvider : IPatrolPointProvider
    {
        private readonly Transform _origin;
        private readonly float _radius;
        private readonly float _sampleDistance;
        private readonly int _maxAttempts;
 
        public RandomPatrolPointProvider(Transform origin, float radius, float sampleDistance = 2f, int maxAttempts = 10)
        {
            _origin = origin;
            _radius = radius;
            _sampleDistance = sampleDistance;
            _maxAttempts = maxAttempts;
        }
 
        public bool TryGetNextPoint(Vector3 forwardHint, out Vector3 point)
        {
            for (int i = 0; i < _maxAttempts; i++)
            {
                Vector2 circle = Random.insideUnitCircle * _radius;
                Vector3 candidate = _origin.position + new Vector3(circle.x, 0f, circle.y);
 
                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, _sampleDistance, NavMesh.AllAreas))
                {
                    point = hit.position;
                    return true;
                }
            }
 
            point = _origin.position;
            return false;
        }
    }
}