using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs
{
    public abstract class AttackConfig : ScriptableObject
    {
        [Header("Common")] 
        [Min(0f)] public float Damage;
        [Min(0)] public float AttackDelay = 0f;
        [Min(0)] public float AttackCooldown = 0f;
        [Min(0)] public float HorizontalForceOnHit = 0f;
        [Min(0)] public float VerticalForceOnHit = 0f;
    }
}