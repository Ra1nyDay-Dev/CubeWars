using System.Collections.Generic;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.Brain;
using Project.Scripts.Gameplay.CharacterSystems.Brain.Player;

namespace Project.Scripts.Gameplay.Services.Factories.BrainFactory
{
    public interface IBrainFactory
    {
        CharacterBrain Create(Character character, BrainType brainType);
        IReadOnlyList<CharacterBrain> Brains { get; }
        PlayerCharacterBrain PlayerBrain { get; }
    }
}