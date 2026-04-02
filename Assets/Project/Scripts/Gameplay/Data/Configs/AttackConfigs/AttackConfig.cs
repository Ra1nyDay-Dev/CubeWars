using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs.AttackConfigs
{
    public abstract class AttackConfig : ScriptableObject
    {
        [Header("Common")] 
        [Min(0f)] public float Damage = 10f;
        [Min(0)] public float AttackDelay = 0.5f;
        [Min(0)] public float AttackCooldown = 0.5f;
        [Min(0)] public float HorizontalForceOnHit = 0f;
        [Min(0)] public float VerticalForceOnHit = 0f;
        [Range(1, 2)] public int AttackAnimationsCount = 1;
        public DeathType AnimationOnDeath = DeathType.Default;
    }
}