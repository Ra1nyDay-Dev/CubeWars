using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation;
using Project.Scripts.Gameplay.Data.Configs.AI;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI
{
    public class BotContext
    {
        public Character Character { get; }
        public AiBotConfig Config { get; }
        public NavMeshAgentMovement AgentMovement { get; }
        public IPatrolPointProvider PatrolPointProvider { get; }
 
        public BotContext(Character character, AiBotConfig config)
        {
            Character = character;
            Config = config;
            AgentMovement = new NavMeshAgentMovement(character, config);
            PatrolPointProvider = BuildPatrolProvider(character, config);
        }
 
        private static IPatrolPointProvider BuildPatrolProvider(Character character, AiBotConfig config)
        {
            PatrolGraph graph = Object.FindAnyObjectByType<PatrolGraph>();
            if (graph != null && graph.Points != null && graph.Points.Count > 0)
                return new GraphPatrolPointProvider(character.transform, graph.Points);
 
            return new RandomPatrolPointProvider(
                character.transform,
                config.PatrolRadius,
                config.PatrolSampleDistance);
        }
    }
}