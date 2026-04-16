using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.Data.Configs.WeaponConfigs;
using Project.Scripts.Gameplay.Data.Enums;
using Project.Scripts.Gameplay.WeaponSystems.AttackSystems;

namespace Project.Scripts.Gameplay.WeaponSystems
{
    public interface IWeapon
    {
        Character Owner { get; }
        
        void Construct(
            WeaponConfig config,
            AttackBehaviour primaryAttack,
            AttackBehaviour secondaryAttack,
            Character owner
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