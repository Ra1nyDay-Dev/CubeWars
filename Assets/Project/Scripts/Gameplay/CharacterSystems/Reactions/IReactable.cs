using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Reactions
{
    public interface IReactable
    {
        void GetHitForce(Vector3 hitDirection, float horizontalForceOnHit, float verticalForceOnHit);
    }
}