using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Characters.Movement;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Services.Fabrics.Weapon
{
    public interface IWeaponFabric
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