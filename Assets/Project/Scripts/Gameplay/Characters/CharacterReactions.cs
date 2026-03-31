using System;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters
{
    public class CharacterReactions : MonoBehaviour
    {
        private Character _character;
        private IDamageable _damageble;

        private void Awake()
        {
            _damageble = GetComponent<IDamageable>();
            _character = GetComponent<Character>();
        }

        private void OnEnable()
        {
            _damageble.Damaged += OnHit;
        }

        private void OnDisable()
        {
            _damageble.Damaged -= OnHit;
        }

        private void OnHit(Vector3 hitDirection)
        {
            float horizontalForce  = 6f;
            float upwardForce  = 6f;

            Vector3 localDir = transform.InverseTransformDirection(hitDirection);
            
            Vector3 force =
                -localDir * horizontalForce +
                Vector3.up * upwardForce;
            _character.AddForce(force);
        }
    }
}