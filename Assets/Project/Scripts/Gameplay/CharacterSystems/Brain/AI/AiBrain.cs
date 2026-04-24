using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.States;
using Project.Scripts.Gameplay.Data.Configs.AI;
using Project.Scripts.Infrastructure.StateMachine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI
{
    public class AiBrain : CharacterBrain
    {
        private readonly StateMachine _stateMachine;
        private readonly BotContext _context;
        private readonly PatrolState _patrolState;
 
        public AiBrain(Character character, AiBotConfig config) : base(character)
        {
            _context = new BotContext(character, config);
            _stateMachine = new StateMachine();
 
            _patrolState = new PatrolState(_context);
            _stateMachine.SetState(_patrolState);
 
            Character.RespawnBehaviour.Respawned += OnRespawned;
        }
 
        public override void Dispose()
        {
            Character.RespawnBehaviour.Respawned -= OnRespawned;
            base.Dispose();
        }
 
        protected override void UpdateLogic(float deltaTime) =>
            _stateMachine.Tick(deltaTime);
 
        private void OnRespawned()
        {
            _context.AgentMovement.Warp(Character.transform.position);
            _context.AgentMovement.Stop();
            _stateMachine.SetState(_patrolState);   // перезапуск Enter → новый PickNewDestination
        }
    }
}