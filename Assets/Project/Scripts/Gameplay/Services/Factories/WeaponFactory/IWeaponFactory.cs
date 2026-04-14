using Project.Scripts.Gameplay.CharacterSystems.Movement;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Services.Factories.WeaponFactory
{
    public interface IWeaponFactory
    {
        GameObject CreateWeaponInHands(WeaponType weaponType,
            Transform weaponSlot,
            Transform attackStartPoint,
            Material handsSkinMaterial,
            GameObject selfHitbox, 
            CharacterMovement owner);
        
        GameObject CreateWeaponAtSpawn(WeaponType weaponType, Transform weaponSlot);
    }
}