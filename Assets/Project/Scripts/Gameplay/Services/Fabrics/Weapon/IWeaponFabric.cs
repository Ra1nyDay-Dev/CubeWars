using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Infrastructure.Services.ServiceLocator;
using UnityEngine;

namespace Project.Scripts.Gameplay.Services.Fabrics.Weapon
{
    public interface IWeaponFabric : ISceneService
    {
        GameObject CreateWeaponInHands(WeaponType weaponType,
            Transform weaponSlot,
            Transform overlapAttackStartPoint,
            Material handsSkinMaterial,
            GameObject selfHitbox, 
            Character owner);
    }
}