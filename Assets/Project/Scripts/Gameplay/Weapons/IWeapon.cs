using Project.Scripts.Gameplay.AttackSystems;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public interface IWeapon
    {
        void Construct(AttackBehaviour primaryAttack, AttackBehaviour secondaryAttack, Material handsSkinMaterial);
        void PerformPrimaryAttack();
        void PerformSecondaryAttack();
    }
}