using System.Collections.Generic;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.Brain;

namespace Project.Scripts.Gameplay.Services.Factories.BrainFactory
{
    public interface IBrainFactory
    {
        CharacterBrain Create(Character character, BrainType brainType);
        List<CharacterBrain> Brains { get; }
    }
}