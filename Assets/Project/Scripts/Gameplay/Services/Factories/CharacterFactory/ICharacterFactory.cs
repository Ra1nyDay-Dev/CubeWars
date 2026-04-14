using Project.Scripts.Gameplay.CharacterSystems;
using UnityEngine;

namespace Project.Scripts.Gameplay.Services.Factories.CharacterFactory
{
    public interface ICharacterFactory
    {
        Character Create(Vector3 initialPoint);
    }
}