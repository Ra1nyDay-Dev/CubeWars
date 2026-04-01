using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Data
{
    public class DamageData
    {
        public float Damage { get; }
        public DamageSource DamageSource { get; }
        public DeathType DeathType { get; }
        public WeaponType WeaponType { get; }
        public Vector3 HitDirection { get; }
        public float HorizontalHitForce { get; }
        public float VerticalHitForce { get; }

        public DamageData(
            float damage, 
            DamageSource damageSource,
            DeathType deathType = DeathType.Default,
            WeaponType weaponType = WeaponType.Knife,
            Vector3 hitDirection = default,
            float horizontalHitForce = 0f,
            float verticalHitForce = 0f)
        {
            Damage = damage;
            WeaponType = weaponType;
            DeathType = deathType;
            HitDirection = hitDirection;
            DamageSource = damageSource;
            HorizontalHitForce = horizontalHitForce;
            VerticalHitForce = verticalHitForce;
        }
    }
}