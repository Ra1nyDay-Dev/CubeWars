using System.Collections.Generic;
using Project.Scripts.Gameplay.Data.Configs.AI;
using Project.Scripts.Gameplay.SpawnSystems.WeaponSpawn;
using Project.Scripts.Gameplay.WeaponSystems;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Sensors
{
    public class WeaponSpawnerSensor
    {
        private readonly Transform _agentTransform;
        private readonly AiWeaponPriorityConfig _priorityConfig;
        private readonly float _weaponCheckRadius;
        private readonly IReadOnlyList<WeaponSpawner> _spawners;
 
        public WeaponSpawnerSensor(
            Transform agentTransform,
            IReadOnlyList<WeaponSpawner> spawners,
            AiWeaponPriorityConfig priorityConfig,
            float weaponCheckRadius)
        {
            _agentTransform = agentTransform;
            _spawners = spawners;
            _priorityConfig = priorityConfig;
            _weaponCheckRadius = weaponCheckRadius;
        }
 
        public bool HasBetterWeaponNearby(IWeapon currentWeapon) => 
            FindBestAvailable(_priorityConfig.GetPriority(currentWeapon)) != null;

        public WeaponSpawner FindBestAvailable(int minPriorityExclusive)
        {
            WeaponSpawner bestWeaponSpawner = null;
            int bestPriority = AiWeaponPriorityConfig.UNARMED_PRIORITY;
            float leastSquareDistance = float.MaxValue;
            float squareCheckRadius = _weaponCheckRadius * _weaponCheckRadius;
 
            foreach (WeaponSpawner weaponSpawner in _spawners)
            {
                if (weaponSpawner == null || !weaponSpawner.IsWeaponAvailable)
                    continue;
 
                float squareDistanceToSpawner = (weaponSpawner.transform.position - _agentTransform.position).sqrMagnitude;
                if (squareDistanceToSpawner > squareCheckRadius)
                    continue;
 
                int spawnerWeaponPriority = _priorityConfig.GetPriority(weaponSpawner.WeaponType);
                if (spawnerWeaponPriority <= minPriorityExclusive)
                    continue;
 
                if (spawnerWeaponPriority > bestPriority 
                    || (spawnerWeaponPriority == bestPriority && squareDistanceToSpawner < leastSquareDistance))
                {
                    bestWeaponSpawner = weaponSpawner;
                    bestPriority = spawnerWeaponPriority;
                    leastSquareDistance = squareDistanceToSpawner;
                }
            }
 
            return bestWeaponSpawner;
        }
    }
}