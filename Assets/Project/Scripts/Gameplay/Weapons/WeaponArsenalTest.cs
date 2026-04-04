using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public class WeaponArsenalTest : MonoBehaviour
    {
        [SerializeField] private Character _character;

        private WeaponArsenal _weaponArsenal;
        
        private void Awake()
        {
            _weaponArsenal = _character.GetComponent<WeaponArsenal>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
                _weaponArsenal.ChangeWeapon(WeaponType.Knife);
            if (Input.GetKeyDown(KeyCode.Alpha2)) 
                _weaponArsenal.ChangeWeapon(WeaponType.Hammer);
            if (Input.GetKeyDown(KeyCode.Alpha3)) 
                _weaponArsenal.ChangeWeapon(WeaponType.Pistol);
            if (Input.GetKeyDown(KeyCode.Alpha4)) 
                _weaponArsenal.ChangeWeapon(WeaponType.AK47);
        }
    }
}