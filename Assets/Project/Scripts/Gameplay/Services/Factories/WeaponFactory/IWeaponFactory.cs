using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Services.Factories.WeaponFactory
{
    public interface IWeaponFactory
    {
        GameObject CreateWeaponInHands(WeaponType weaponType,
            Transform weaponSlot,
            Transform attackStartPoint,
            GameObject selfHitbox, 
            Character owner);
        
        GameObject CreateWeaponAtSpawn(WeaponType weaponType, Transform weaponSlot);
    }
}