using Project.Scripts.Gameplay.AttackSystems;
using Project.Scripts.Gameplay.Data.Configs;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private GameObject[] _hands;
        
        private AttackBehaviour _primaryAttack;
        private AttackBehaviour _secondaryAttack;

        public void Construct(AttackBehaviour primaryAttack, AttackBehaviour secondaryAttack, Material handsSkinMaterial)
        {
            _primaryAttack = primaryAttack;
            _secondaryAttack = secondaryAttack;
            ApplyHandsSkinMaterial(handsSkinMaterial);
        }

        public void PerformPrimaryAttack()
        {
            if (_primaryAttack != null)
                _primaryAttack.PerformAttack();
        }

        public void PerformSecondaryAttack()
        {
            if (_secondaryAttack != null)
                _secondaryAttack.PerformAttack();
        }

        private void ApplyHandsSkinMaterial(Material material)
        {
            foreach (GameObject hand in _hands) 
                hand.GetComponent<Renderer>().material = material;
        }
        
        
    }
}