using System;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters
{
    public class CharacterReactions : MonoBehaviour, IReactable
    {
        private Character _character;
        private IDamageable _damageable;

        private void Awake()
        {
            _character = GetComponent<Character>();
            _damageable = GetComponent<IDamageable>();

            _damageable.Damaged += OnDamaged;
        }

        private void OnDestroy() => 
            _damageable.Damaged -= OnDamaged;

        private void OnDamaged(DamageData damageData) => 
            GetHitForce(damageData.HitDirection, damageData.HorizontalHitForce, damageData.VerticalHitForce);

        public void GetHitForce(Vector3 hitDirection, float horizontalForceOnHit, float verticalForceOnHit)
        {
            Vector3 localDir = transform.InverseTransformDirection(hitDirection);
            
            Vector3 force =
                -localDir * horizontalForceOnHit +
                Vector3.up * verticalForceOnHit;
            _character.AddForce(force);
        }
    }
}