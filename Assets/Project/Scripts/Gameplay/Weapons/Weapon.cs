using System;
using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.Characters;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private GameObject[] _hands;
        
        public Character Owner {get; private set;}
        public event Action PrimaryAttackHappened;
        public event Action SecondaryAttackHappened;
        
        private AttackBehaviour _primaryAttack;
        private AttackBehaviour _secondaryAttack;

        public void Construct(
            AttackBehaviour primaryAttack, 
            AttackBehaviour secondaryAttack, 
            Character owner,
            Material handsSkinMaterial)
        {
            _primaryAttack = primaryAttack;
            _secondaryAttack = secondaryAttack;
            Owner = owner;
            ApplyHandsSkinMaterial(handsSkinMaterial);
        }

        public void PerformPrimaryAttack()
        {
            if (_primaryAttack != null)
            {
                _primaryAttack.PerformAttack();
                PrimaryAttackHappened?.Invoke();
            }
        }

        public void PerformSecondaryAttack()
        {
            if (_secondaryAttack != null)
            {
                _secondaryAttack.PerformAttack();
                SecondaryAttackHappened?.Invoke();
            }
        }

        private void ApplyHandsSkinMaterial(Material material)
        {
            foreach (GameObject hand in _hands) 
                hand.GetComponent<Renderer>().material = material;
        }
        
        
    }
}