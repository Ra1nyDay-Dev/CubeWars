using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.PointProvider
{
    public interface IPatrolPointProvider
    {
        bool TryGetNextPoint(Vector3 moveDirection, out Vector3 point);
    }
}