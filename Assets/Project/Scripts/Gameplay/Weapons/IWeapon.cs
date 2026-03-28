using System;
using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.Characters;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public interface IWeapon
    {
        void Construct(AttackBehaviour primaryAttack, AttackBehaviour secondaryAttack, Character owner,
            Material handsSkinMaterial);
        event Action PrimaryAttackEnded;
        event Action SecondaryAttackEnded;
        event Action PrimaryAttackStarted;
        event Action SecondaryAttackStarted;
        Awaitable PerformPrimaryAttack();
        Awaitable PerformSecondaryAttack();
    }
}