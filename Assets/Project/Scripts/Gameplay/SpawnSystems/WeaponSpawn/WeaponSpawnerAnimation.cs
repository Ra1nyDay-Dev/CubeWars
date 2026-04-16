using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Gameplay.SpawnSystems.WeaponSpawn
{
    public class WeaponSpawnerAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _weaponSlot;
        
        [SerializeField] private float _fullRotateTime = 3f;
        [SerializeField] private float _hoverCycleTime = 1.2f;
        [SerializeField] private float _hoverVerticalOffset = 0.5f;
        
        private Sequence _weaponSpinSequence;
        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = _weaponSlot.localPosition;
            SetUpTweens();
        }

        public void StartAnimation()
        {
            StopAnimation();
            _weaponSpinSequence?.Restart();
        }

        public void StopAnimation()
        {
            _weaponSpinSequence?.Pause();
            Reset();
        }

        private void Reset()
        {
            _weaponSlot.localPosition = _startPosition;
            _weaponSlot.localRotation = Quaternion.identity;
        }

        private void SetUpTweens()
        {
            _weaponSpinSequence = DOTween.Sequence();
            
            _weaponSpinSequence.Join(
                _weaponSlot
                    .DORotate(new Vector3(0, 360, 0), _fullRotateTime, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
            );
            
            _weaponSpinSequence.Join(
                _weaponSlot
                    .DOLocalMoveY(_startPosition.y + _hoverVerticalOffset, _hoverCycleTime)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(2, LoopType.Yoyo)
            );
            
            _weaponSpinSequence.SetLoops(-1, LoopType.Restart);
        }

        private void OnDestroy() => 
            _weaponSpinSequence.Kill();
    }
}
