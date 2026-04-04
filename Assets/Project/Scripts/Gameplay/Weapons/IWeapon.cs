using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public interface IWeapon
    {
        void Construct(
            WeaponConfig config,
            AttackBehaviour primaryAttack,
            AttackBehaviour secondaryAttack,
            Character owner,
            Material handsSkinMaterial
            );

        WeaponType WeaponType { get; }
        AttackBehaviour PrimaryAttack { get; }
        AttackBehaviour SecondaryAttack { get; }
        
        event Action PrimaryAttackStarted;
        event Action PrimaryAttackEnded;
        event Action SecondaryAttackStarted;
        event Action SecondaryAttackEnded;

        UniTask StartPrimaryAttack();
        UniTask StartSecondaryAttack();
        void StopPrimaryAttack();
        void StopSecondaryAttack();
    }
}