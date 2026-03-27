using Project.Scripts.Gameplay.AttackSystems.OverlapAttack;
using Project.Scripts.Gameplay.Data.Configs;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public class Weapon
    {
        [SerializeField] private OverlapAttack primaryAttack;
        [SerializeField] private OverlapAttack secondaryAttack;
        [SerializeField] private WeaponConfig _weaponConfig;

        
        
    }
}