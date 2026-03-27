using Project.Scripts.Gameplay.Characters;
using Project.Scripts.Gameplay.Data.Enums;
using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public class WeaponArsenalTest : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private WeaponType _spawnedWeaponType = WeaponType.Knife;

        private WeaponArsenal _weaponArsenal;
        
        private void Awake()
        {
            _weaponArsenal = _character.GetComponent<WeaponArsenal>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)) 
                _weaponArsenal.ChangeWeapon(_spawnedWeaponType);
        }
    }
}