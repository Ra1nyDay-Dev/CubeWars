using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs.WeaponConfigs
{
    [CreateAssetMenu(fileName = "RangeWeaponConfig", menuName = "Configs/Weapon/RangeWeaponConfig")]
    public class RangeWeaponConfig : WeaponConfig
    {
        [Header("Magazine")]
        public bool IsReloadable = true;
        [Min(1)] public int MaxAmmoInMagazine = 1;
        [Min(0)] public float ReloadTime = 1f;
        
        [Header("Ammo")]
        public bool InfiniteAmmo = true;
        [Min(1)] public int MaxAmmo = 1;
        [Min(1)] public int AmmoPerPrimaryShot = 1;
        [Min(0)] public int AmmoPerSecondaryShot = 0;
    }
}   