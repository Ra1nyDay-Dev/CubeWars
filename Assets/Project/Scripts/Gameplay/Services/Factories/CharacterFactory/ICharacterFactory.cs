using System.Collections.Generic;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.Brain;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.Services.Factories.CharacterFactory
{
    public interface ICharacterFactory
    {
        Character Create(Vector3 position, Quaternion rotation, Material material, BrainType brainType);
        List<Character> Characters { get; }
    }
}