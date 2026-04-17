using System;
using Project.Scripts.Gameplay.CharacterSystems.HealthSystems;
using Project.Scripts.Gameplay.CharacterSystems.Movement;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Reactions
{
    public class CharacterReactions : MonoBehaviour, IReactable
    {
        private CharacterMovement _characterMovement;
        private IDamageable _damageable;

        private void Awake()
        {
            _characterMovement = GetComponent<CharacterMovement>();
            _damageable = GetComponent<IDamageable>();
            _damageable.Damaged += OnDamaged;
        }

        private void OnDestroy() => 
            _damageable.Damaged -= OnDamaged;


        private void OnDamaged(DamageData damageData) => 
            GetHitForce(damageData.HitDirection, damageData.HorizontalHitForce, damageData.VerticalHitForce);

        public void GetHitForce(Vector3 hitDirection, float horizontalForceOnHit, float verticalForceOnHit)
        {
            Vector3 force = hitDirection.normalized * horizontalForceOnHit 
                            + Vector3.up * verticalForceOnHit;
            
            _characterMovement.AddForce(force);
        }
    }
}