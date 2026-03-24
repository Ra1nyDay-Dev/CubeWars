using System;
using UnityEngine;

namespace Project.Scripts.Characters.CubeGuy.Movement
{
    public class DirectionalRotation
    {
        public event Action<Quaternion> RotationChanged;
        
        private readonly Transform _transform;
        private readonly float _rotationSpeed;
        private Vector3 _currentDirection;
        private Quaternion _lastRotation;

        public DirectionalRotation(Transform transform, float rotationSpeed)
        {
            _transform = transform;
            _rotationSpeed = rotationSpeed;
        }
        
        public Quaternion CurrentRotation => 
            _transform.rotation;
        
        public void SetDirection(Vector3 direction)
        {
            _currentDirection = direction;
        }

        public void Update(float deltaTime)
        {
            CheckRotationChange();
            
            if (_currentDirection.magnitude < 0.05f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(_currentDirection.normalized);
            _transform.rotation = Quaternion.Slerp(CurrentRotation, targetRotation, _rotationSpeed * deltaTime);
        }

        private void CheckRotationChange()
        {
            if (Quaternion.Angle(_lastRotation, CurrentRotation) > 0.1f)
            {
                RotationChanged?.Invoke(CurrentRotation);
                _lastRotation = CurrentRotation;
            }
        }
    }
}