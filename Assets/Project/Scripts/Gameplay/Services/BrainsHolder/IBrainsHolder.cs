using Project.Scripts.Gameplay.CharacterSystems.Brain;

namespace Project.Scripts.Gameplay.Services.BrainsHolder
{
    public interface IBrainsHolder
    {
        void Add(CharacterBrain brain);
        void Remove(CharacterBrain brain);
        void Clear();
    }
}