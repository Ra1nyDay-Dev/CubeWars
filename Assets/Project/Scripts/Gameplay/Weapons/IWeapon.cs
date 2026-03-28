using System;
using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.Characters;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public interface IWeapon
    {
        void Construct(AttackBehaviour primaryAttack, AttackBehaviour secondaryAttack, Character Owner,
            Material handsSkinMaterial);
        void PerformPrimaryAttack();
        void PerformSecondaryAttack();
        event Action PrimaryAttackHappened;
        event Action SecondaryAttackHappened;
    }
}