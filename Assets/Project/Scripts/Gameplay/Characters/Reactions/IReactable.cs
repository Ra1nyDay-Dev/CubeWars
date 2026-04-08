using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.Reactions
{
    public interface IReactable
    {
        void GetHitForce(Vector3 hitDirection, float horizontalForceOnHit, float verticalForceOnHit);
    }
}