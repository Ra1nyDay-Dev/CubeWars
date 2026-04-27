using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Gameplay.Services.Factories.CharacterFactory;
using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Sensors
{
    public class EnemySensor
    {
        private readonly Character _selfCharacter;
        private readonly float _detectionRadius;
        private readonly float _maxVerticalDelta;
        private readonly float _eyeHeight;
        private readonly LayerMask _obstacleLayerMask;
        private readonly IReadOnlyList<Character> _enemyCharacters;
 
        public EnemySensor(
            Character selfCharacter,
            IReadOnlyList<Character> characters,
            float detectionRadius,
            float maxVerticalDelta,
            float eyeHeight,
            LayerMask obstacleLayerMask)
        {
            _selfCharacter = selfCharacter;
            _detectionRadius = detectionRadius;
            _maxVerticalDelta = maxVerticalDelta;
            _eyeHeight = eyeHeight;
            _obstacleLayerMask = obstacleLayerMask;

            _enemyCharacters = new List<Character>(
                characters.Where(character => character != selfCharacter));
        }
 
        public bool HasVisibleEnemy() =>
            FindNearestVisibleEnemy() != null;
 
        public Character FindNearestVisibleEnemy()
        {
            Character nearest = null;
            float leastSquareDistanceToEnemy = float.MaxValue;
 
            foreach (Character enemyCharacter in _enemyCharacters)
            {
                if (!IsVisible(enemyCharacter))
                    continue;
                
                float squareDistanceToEnemy = (enemyCharacter.transform.position - _selfCharacter.transform.position).sqrMagnitude;
 
                if (squareDistanceToEnemy < leastSquareDistanceToEnemy)
                {
                    nearest = enemyCharacter;
                    leastSquareDistanceToEnemy = squareDistanceToEnemy;
                }
            }
 
            return nearest;
        }
 
        public bool IsVisible(Character enemyCharacter)
        {
            float squareDetectionRadius = _detectionRadius * _detectionRadius;
            
            if (enemyCharacter.RespawnBehaviour.IsDead)
                return false;
 
            Vector3 positionDelta = enemyCharacter.transform.position - _selfCharacter.transform.position;
            if (Mathf.Abs(positionDelta.y) > _maxVerticalDelta)
                return false;
 
            return positionDelta.sqrMagnitude <= squareDetectionRadius;
        }
 
        public bool HasLineOfSight(Character enemyCharacter)
        {
            if (enemyCharacter == null)
                return false;
 
            Vector3 selfVisorPosition = _selfCharacter.transform.position + Vector3.up * _eyeHeight;
            Vector3 enemyPosition = enemyCharacter.transform.position + Vector3.up * _eyeHeight;
            Vector3 direction = enemyPosition - selfVisorPosition;
            float distance = direction.magnitude;
            
            if (distance < 0.1f)
                return true;
 
            return !Physics.Raycast(
                selfVisorPosition,
                direction / distance,
                distance,
                _obstacleLayerMask,
                QueryTriggerInteraction.Ignore);
        }
    }
}