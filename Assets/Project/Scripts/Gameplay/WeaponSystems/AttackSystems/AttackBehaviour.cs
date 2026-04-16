using Project.Scripts.Gameplay.Data.Enums;

namespace Project.Scripts.Gameplay.WeaponSystems.AttackSystems
{
    public abstract class AttackBehaviour
    {
        public float Damage { get; protected set; }
        public float AttackDelay { get; protected set; }
        public float AttackInterval { get; protected set; }
        public float HorizontalForceOnHit { get; protected set; }
        public float VerticalForceOnHit { get; protected set; }
        public int AttackAnimationsCount { get; protected set; }
        public DeathType AttackDeathType { get; protected set; }
        public WeaponType WeaponType {get; protected set;}
        public bool HoldingButtonContinuesAttack { get; protected set; }
        
        
        public abstract void PerformAttack();
    }
}