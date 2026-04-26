using System;
using System.Collections.Generic;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Gameplay.WeaponSystems;
using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs.AI
{
    [CreateAssetMenu(fileName = "AiWeaponPriorityConfig", menuName = "Configs/AI/Weapon Priority")]
    public class AiWeaponPriorityConfig : ScriptableObject
    {
        public const int UNARMED_PRIORITY = -1;
        
        [SerializeField] private List<WeaponPriority> _weaponsPriority = new();
 
        private Dictionary<WeaponType, int> _cache;
 
        public int GetPriority(WeaponType weaponType)
        {
            FillCache();

            if (_cache.TryGetValue(weaponType, out int priority))
                return priority;
            
            Debug.LogError($"No priority in AiWeaponPriorityConfig config for weapon {weaponType}");
            return UNARMED_PRIORITY;
        }

        public int GetPriority(IWeapon weapon) => 
            weapon == null ? UNARMED_PRIORITY : GetPriority(weapon.WeaponType);

        public Dictionary<WeaponType, int> GetPriorities()
        {
            FillCache();
            return new Dictionary<WeaponType, int>(_cache);
        }
 
        private void FillCache()
        {
            if (_cache != null && _cache.Count == _weaponsPriority.Count)
                return;
 
            _cache = new Dictionary<WeaponType, int>(_weaponsPriority.Count);
            
            foreach (WeaponPriority weaponPriority in _weaponsPriority)
                _cache[weaponPriority.WeaponType] = weaponPriority.Priority;
        }
 
        private void OnValidate() =>
            _cache = null;
 
        private void Reset()
        {
            _weaponsPriority = new List<WeaponPriority>
            {
                new() { WeaponType = WeaponType.Minigun, Priority = 6 },
                new() { WeaponType = WeaponType.AK47,    Priority = 5 },
                new() { WeaponType = WeaponType.Shotgun, Priority = 4 },
                new() { WeaponType = WeaponType.Pistol,  Priority = 3 },
                new() { WeaponType = WeaponType.Hammer,  Priority = 2 },
                new() { WeaponType = WeaponType.Knife,   Priority = 1 },
            };
        }
    }
}