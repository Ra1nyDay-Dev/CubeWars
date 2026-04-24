using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation
{
    public interface IPatrolPointProvider
    {
        bool TryGetNextPoint(Vector3 forwardHint, out Vector3 point);
    }
}