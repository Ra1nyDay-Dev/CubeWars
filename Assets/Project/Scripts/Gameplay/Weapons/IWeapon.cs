using System;
using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public interface IWeapon
    {
        void Construct(
            AttackBehaviour primaryAttack,
            AttackBehaviour secondaryAttack,
            Character owner,
            WeaponType weaponType,
            Material handsSkinMaterial
            );

        WeaponType WeaponType { get; }
        AttackBehaviour PrimaryAttack { get; }
        AttackBehaviour SecondaryAttack { get; }
        
        event Action PrimaryAttackStarted;
        event Action PrimaryAttackEnded;
        event Action SecondaryAttackStarted;
        event Action SecondaryAttackEnded;
        
        Awaitable PerformPrimaryAttack();
        Awaitable PerformSecondaryAttack();
    }
}