using Project.Scripts.Gameplay.CharacterSystems.Animations;
using Project.Scripts.Gameplay.CharacterSystems.HealthSystems;
using Project.Scripts.Gameplay.CharacterSystems.Interactions;
using Project.Scripts.Gameplay.CharacterSystems.Movement;
using Project.Scripts.Gameplay.CharacterSystems.Reactions;
using Project.Scripts.Gameplay.Weapons;
using Project.Scripts.UI.Elements;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems
{
    public class Character : MonoBehaviour
    {
        public CharacterMovement Movement { get; private set; }
        public WeaponArsenal WeaponArsenal { get; private set; }
        public CharacterAnimations Animations { get; private set; }
        public CharacterReactions Reactions { get; private set; }
        public CharacterUI CharacterUI { get; private set; }
        public IHealth Health { get; private set; }
        public Death Death { get; private set; }
        public InteractorUnit Interactor { get; private set; }
        
        private void Awake()
        {
            Movement = GetComponent<CharacterMovement>();
            WeaponArsenal = GetComponent<WeaponArsenal>();
            Animations = GetComponent<CharacterAnimations>();
            Reactions = GetComponent<CharacterReactions>();
            CharacterUI = GetComponent<CharacterUI>();
            Health = GetComponent<IHealth>();
            Death = GetComponent<Death>();
            Interactor = GetComponent<InteractorUnit>();
        }

        
    }
}