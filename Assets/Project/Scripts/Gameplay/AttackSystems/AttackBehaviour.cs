using Project.Scripts.Gameplay.Weapons;
using UnityEngine;

namespace Project.Scripts.Gameplay.AttackSystems
{
    public abstract class AttackBehaviour : MonoBehaviour, IWeapon
    {
        public abstract void PerformAttack();
    }
}