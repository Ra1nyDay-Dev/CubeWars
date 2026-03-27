using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/Weapon/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        public GameObject WeaponPrefab;

        [Header("Primary Attack")] 
        public AttackType PrimaryAttackType;
        public AttackConfig PrimaryAttackBehaviourConfig;

        [Header("Secondary Attack (optional)")]
        public AttackType SecondaryAttackType;
        public AttackConfig SecondaryAttackBehaviourConfig;
    }
}