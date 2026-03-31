using System;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters
{
    public class CharacterReactions : MonoBehaviour, IReactable
    {
        private Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();
        }
        
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