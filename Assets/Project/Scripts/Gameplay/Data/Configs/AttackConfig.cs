using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs
{
    public abstract class AttackConfig : ScriptableObject
    {
        [Header("Common")] 
        [Min(0f)] public float Damage;
    }
}