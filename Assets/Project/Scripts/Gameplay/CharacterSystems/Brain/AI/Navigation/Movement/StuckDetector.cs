using UnityEngine;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Navigation.Movement
{
    public class StuckDetector
    {
        private readonly Transform _targetTransform;
 
        private Vector3 _lastCheckedPosition;
        private float _stuckCheckTimer;
        private readonly float _stuckCheckInterval;
        private readonly float _stuckDistanceThreshold;

        public StuckDetector(Transform targetTransform, float stuckCheckInterval, float stuckDistanceThreshold)
        {
            _targetTransform = targetTransform;
            _stuckCheckInterval = stuckCheckInterval;
            _stuckDistanceThreshold = stuckDistanceThreshold;
            ResetTimer();
        }

        public bool IsStuckInThisFrame(float deltaTime)
        {
            _stuckCheckTimer += deltaTime;
            if (_stuckCheckTimer < _stuckCheckInterval)
                return false;
 
            float threshold = _stuckDistanceThreshold;
            bool stuck = (_targetTransform.position - _lastCheckedPosition).sqrMagnitude < threshold * threshold;

            return stuck;
        }

        public void ResetTimer()
        {
            _stuckCheckTimer = 0f;
            _lastCheckedPosition = _targetTransform.position;
        }
    }
}