using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public class WeaponArsenal : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponSlot;
        [SerializeField] private GameObject _overlapAttackStartPoint;
        
        public IWeapon CurrentWeapon { get; private set; }

        public void ChangeWeapon(WeaponType weaponType)
        {
            UnequipWeapon();
            EquipWeapon(weaponType);
        }
        
        
        private void EquipWeapon(WeaponType weaponType)
        {
            // toDo: create WeaponFabric and inject it
        }

        private void UnequipWeapon()
        {
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                IWeapon weapon = child.GetComponent<IWeapon>();
                if (weapon != null)
                    Destroy(child.gameObject);
            }

            CurrentWeapon = null;
        }
    }
}