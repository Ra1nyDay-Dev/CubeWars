using UnityEngine;

namespace Project.Scripts.Gameplay.Services.Fabrics.Particle
{
    public class ParticleService : MonoBehaviour, IParticleService
    {
        [SerializeField, Min(0f)] private float _hitEffectDestroyDelay = 2f;
        
        public void SpawnEffect(ParticleSystem effect, Vector3 position, Quaternion rotation)
        {
            
        }
    }
}