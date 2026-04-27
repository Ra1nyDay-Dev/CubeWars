using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Combat
{
    public class AttackPositionProvider
    {
        private const int MAX_PICK_ATTEMPTS = 10;
        
        private readonly float _sampleDistance;
 
        public AttackPositionProvider(float sampleDistance) => 
            _sampleDistance = sampleDistance;

        public bool TryGetRandomPointAround(Vector3 center, float minRadius, float maxRadius, out Vector3 point)
        {
            for (int i = 0; i < MAX_PICK_ATTEMPTS; i++)
            {
                float angle = Random.Range(0f, Mathf.PI * 2f);
                float radius = Random.Range(minRadius, maxRadius);
                Vector3 candidate = center + new Vector3(
                    Mathf.Cos(angle) * radius,
                    0f,
                    Mathf.Sin(angle) * radius);
 
                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, _sampleDistance, NavMesh.AllAreas))
                {
                    point = hit.position;
                    return true;
                }
            }
 
            point = center;
            return false;
        }
    }
}