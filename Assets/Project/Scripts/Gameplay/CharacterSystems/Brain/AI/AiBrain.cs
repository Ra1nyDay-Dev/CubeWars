using System.Collections.Generic;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.Movement;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.PatrolPoints;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.PointProvider;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Sensors;
using Project.Scripts.Gameplay.CharacterSystems.Brain.AI.States;
using Project.Scripts.Gameplay.Data.Configs.AI;
using Project.Scripts.Gameplay.SpawnSystems.WeaponSpawn;
using Project.Scripts.Infrastructure.StateMachine;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI
{
    public class AiBrain : CharacterBrain
    {
        private readonly AiWeaponPriorityConfig _weaponPriorityConfig;
        private readonly NavMeshAgentMovement _agentMovement;
        private readonly IPatrolPointProvider _patrolPointProvider;
        private readonly WeaponSpawnerSensor _weaponSpawnerSensor;
        private readonly StuckDetector _stuckDetector;
        
        private readonly StateMachine _stateMachine;
        private PatrolState _patrolState;
        private TakeWeaponState _takeWeaponState;

        public AiBrain(
            Character character,
            BotConfig botConfig,
            AiWeaponPriorityConfig weaponPriorityConfig,
            IReadOnlyList<WeaponSpawner> weaponSpawners)
            : base(character)
        {
            _weaponPriorityConfig = weaponPriorityConfig;
            
            _agentMovement = Character.GetComponent<NavMeshAgentMovement>();
            _agentMovement.Construct(botConfig.PatrolArriveDistance);
            _agentMovement.Initialize();
            
            _patrolPointProvider = CreatePatrolProvider(botConfig.PointSearchRadius, botConfig.PatrolSampleDistance);
            
            _weaponSpawnerSensor = new WeaponSpawnerSensor(
                character.transform,
                weaponSpawners,
                weaponPriorityConfig,
                botConfig.WeaponCheckRadius);
            
            _stuckDetector = new StuckDetector(
                character.transform,
                botConfig.StuckCheckInterval,
                botConfig.StuckDistanceThreshold);
            
            _stateMachine = new StateMachine();
            CreateSates();
            CreateStateTransitions();
            _stateMachine.SetState(_patrolState);
        }

        protected override void UpdateLogic(float deltaTime) =>
            _stateMachine.Tick(deltaTime);

        private void CreateSates()
        {
            _patrolState = new PatrolState(Character.Movement, _agentMovement, _stuckDetector, _patrolPointProvider);
            _takeWeaponState = new TakeWeaponState(Character, _agentMovement, _stuckDetector,
                _weaponSpawnerSensor, _weaponPriorityConfig);
        }

        private void CreateStateTransitions()
        {
            _stateMachine.AddTransition(
                _patrolState,
                _takeWeaponState,
                () => _weaponSpawnerSensor.HasBetterWeaponNearby(Character.WeaponArsenal.CurrentWeapon));
            
            _stateMachine.AddTransition(
                _takeWeaponState,
                _patrolState,
                () => !_weaponSpawnerSensor.HasBetterWeaponNearby(Character.WeaponArsenal.CurrentWeapon));
        }
        
        private IPatrolPointProvider CreatePatrolProvider(float pointSearchRadius, float patrolSampleDistance)
        {
            PatrolGraph graph = Object.FindAnyObjectByType<PatrolGraph>();
            
            if (graph != null && graph.Points != null && graph.Points.Count > 0)
            {
                return new GraphPatrolPointProvider(
                    Character.transform,
                    graph.Points,
                    pointSearchRadius);
            }
            
            Debug.LogWarning("Patrol graph not found. Used random bots movement");
 
            return new RandomPatrolPointProvider(
                Character.transform,
                pointSearchRadius,
                patrolSampleDistance);
        }

        protected override void OnCharacterRespawned()
        {
            base.OnCharacterRespawned();
            _agentMovement.Warp(Character.transform.position);
            _agentMovement.Stop();
            _stateMachine.SetState(_patrolState);
        }
    }
}