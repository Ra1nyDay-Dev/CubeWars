using UnityEngine;

namespace Project.Scripts.Gameplay.Characters
{
    public interface IReactable
    {
        void GetHitForce(Vector3 hitDirection, float horizontalForceOnHit, float verticalForceOnHit);
    }
}