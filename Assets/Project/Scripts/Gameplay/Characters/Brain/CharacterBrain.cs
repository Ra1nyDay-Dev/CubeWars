namespace Project.Scripts.Gameplay.Characters.Brain
{
    public abstract class CharacterBrain
    {
        public bool IsEnabled { get; protected set; } = false;
        
        public virtual void Enable() => 
            IsEnabled = true;
        
        public virtual void Disable() => 
            IsEnabled = false;

        public void Update(float deltaTime)
        {
            if (!IsEnabled)
                return;
            
            UpdateLogic(deltaTime);
        }

        protected abstract void UpdateLogic(float deltaTime);
    }
}