using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.Services.Factories.CharacterFactory
{
    public interface ICharacterFactory
    {
        Character Create(InitialPointData initialPointData);
    }
}