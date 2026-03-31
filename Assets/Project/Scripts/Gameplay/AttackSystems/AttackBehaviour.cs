using UnityEngine;

namespace Project.Scripts.Gameplay.AttackSystems
{
    public abstract class AttackBehaviour
    {
        public float Damage { get; protected set; }
        public float AttackDelay { get; protected set; }
        public float AttackCooldown { get; protected set; }
        public float HorizontalForceOnHit { get; protected set; }
        public float VerticalForceOnHit { get; protected set; }
        
        public abstract void PerformAttack();
    }
}