namespace Project.Scripts.Gameplay.CharacterSystems.Brain
{
    
    public class EmptyCharacterBrain : CharacterBrain
    {
        public EmptyCharacterBrain(Character character) : base(character)
        {
        }

        protected override void UpdateLogic(float deltaTime)
        {
            // Just as target for debug
        }
    }
}