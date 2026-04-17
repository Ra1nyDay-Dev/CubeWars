using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.Gameplay.CharacterSystems.HealthSystems;
using Project.Scripts.Gameplay.CharacterSystems.Movement;
using Project.Scripts.Gameplay.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Gameplay.CharacterSystems.Animations
{
    public class CharacterEyesAnimations : MonoBehaviour
    {
        [SerializeField] private Transform _leftEye;
        [SerializeField] private Transform _rightEye;
        [SerializeField] private Transform _leftEyeDead;
        [SerializeField] private Transform _rightEyeDead;
        [SerializeField] private Transform _leftPupil;
        [SerializeField] private Transform _rightPupil;

        [SerializeField] private float _minBlinkInterval = 1.5f;
        [SerializeField] private float _maxBlinkInterval = 3f;
        [SerializeField] private float _minPupilChangePositionInterval = 0.3f;
        [SerializeField] private float _maxPupilChangePositionInterval = 3f;
        [SerializeField] private List<Vector3> _pupilPositions = new List<Vector3>();
        
        private CharacterMovement _characterMovement;
        private IDamageable _damageable;
        private Vector3 _pupilDefaultLocalPosition;
        
        private Coroutine _currentPupilsState;
        private Coroutine _currentBlinkState;
        private Tween _blinkTween;
        private Tween _pupilTween;
        private Sequence _hitSequence;
        private RespawnBehaviour _respawnBehaviour;
        private Vector3 _pupilDefaultScale;
        private Vector3 _eyeDefaultScale;

        private void Awake()
        {
            _characterMovement = GetComponentInParent<CharacterMovement>();
            _damageable = _characterMovement.GetComponent<IDamageable>();
            _respawnBehaviour = _characterMovement.GetComponent<RespawnBehaviour>();
            _pupilDefaultLocalPosition = _leftPupil.localPosition;
            _pupilDefaultScale = _leftPupil.localScale;
            _eyeDefaultScale = _leftEye.localScale;

            SetUpBlinkTween();
            SetUpHitSequence();
            SubscribeToEvents();
        }

        private void OnEnable() => 
            SetIdleAnimation();

        private void SubscribeToEvents()
        {
            _damageable.Damaged += OnDamaged;
            _respawnBehaviour.Dead += OnDie;
            _respawnBehaviour.Respawned += OnRespawn;
        }

        private void UnsubscribeFromEvents()
        {
            _damageable.Damaged -= OnDamaged;
            _respawnBehaviour.Dead -= OnDie;
            _respawnBehaviour.Respawned -= OnRespawn;
        }

        private void OnDestroy()
        {
            StopAllTweens();
            UnsubscribeFromEvents();
        }

        public void Blink() => 
            _blinkTween.Restart();

        public void SetIdleAnimation()
        {
            if (!isActiveAndEnabled)
                return;
            
            if (_currentPupilsState != null)
                StopCoroutine(_currentPupilsState);
            
            if (_currentBlinkState != null)
                StopCoroutine(_currentBlinkState);
            
            _currentPupilsState = StartCoroutine(PupilsAnimation());
            _currentBlinkState = StartCoroutine(BlinkAnimation());
        }

        private void StopAllTweens()
        {
            _blinkTween?.Kill();
            _pupilTween?.Kill();
            _hitSequence?.Kill();
        }

        private void SetUpBlinkTween()
        {
            _blinkTween = _leftEye.DOScaleY(0f, 0.2f)
                .OnUpdate(() =>
                {
                    _rightEye.localScale = _leftEye.localScale;
                })
                .SetLoops(2, LoopType.Yoyo)
                .SetAutoKill(false)
                .SetEase(Ease.InOutQuint);
        }

        private void SetUpHitSequence()
        {
            Vector3 smallPupilsScale = new Vector3(_leftEye.localScale.x, 0.4f, 0.4f);
            Vector3 defaultPupilLocalScale = _leftPupil.localScale;

            _hitSequence = DOTween.Sequence()
                .SetAutoKill(false);

            _hitSequence.Append(
                _leftPupil.DOScale(smallPupilsScale, 0.1f)
                    .OnUpdate(() => _rightPupil.localScale = _leftPupil.localScale));

            _hitSequence.AppendInterval(0.1f);
            _hitSequence.AppendCallback(Blink);
            _hitSequence.AppendInterval(0.05f);
            _hitSequence.AppendCallback(Blink);
            
            _hitSequence.Append(
                _leftPupil.DOScale(defaultPupilLocalScale, 0.1f)
                    .OnUpdate(() => _rightPupil.localScale = _leftPupil.localScale));
        }

        private void SetPupilsPositions(Vector3 position)
        {
            _leftPupil.transform.localPosition = position;
            _rightPupil.transform.localPosition = position;
        }

        private IEnumerator BlinkAnimation()
        {
            while (true)
            {
                float waitBeforeBlink = Random.Range(_minBlinkInterval, _maxBlinkInterval);
                yield return new WaitForSeconds(waitBeforeBlink);
                Blink();
            }
        }

        private IEnumerator PupilsAnimation()
        {
            while (true)
            {
                float waitBeforeChangePosition = Random.Range(_minPupilChangePositionInterval, _maxPupilChangePositionInterval);
                Vector3 pupilPosition = _pupilPositions[Random.Range(0, _pupilPositions.Count)];
                yield return new WaitForSeconds(waitBeforeChangePosition);
                MovePupils(pupilPosition);
            }
        }

        private void MovePupils(Vector3 target)
        {
            _pupilTween?.Kill();
            _pupilTween  = DOTween.To(
                    () => _leftPupil.localPosition,
                    x =>
                    {
                        _leftPupil.localPosition = x;
                        _rightPupil.localPosition = x;
                    },
                    target,
                    0.1f
                ).SetEase(Ease.OutQuad)
                .Play();
        }

        private void OnDamaged(DamageData damageData)
        {
            StopAllCoroutines();
            MovePupils(_pupilDefaultLocalPosition);
            _hitSequence.Restart();
            StartCoroutine(ReturnToIdle());
        }

        private IEnumerator ReturnToIdle()
        {
            if (_hitSequence != null)
                yield return _hitSequence.WaitForCompletion();

            if (isActiveAndEnabled)
                SetIdleAnimation();
        }

        private void OnDie(DamageData damageData)
        {
            StopAllCoroutines();
            StopAllTweens();
            _currentBlinkState = null;
            _currentPupilsState = null;
            _leftEye.gameObject.SetActive(false);
            _rightEye.gameObject.SetActive(false);
            _leftEyeDead.gameObject.SetActive(true);
            _rightEyeDead.gameObject.SetActive(true);
        }
        
        private void OnRespawn()
        {
            _leftPupil.localScale = _pupilDefaultScale;
            _rightPupil.localScale = _pupilDefaultScale;
            _leftEye.localScale = _eyeDefaultScale;
            _rightEye.localScale = _eyeDefaultScale;
            SetPupilsPositions(_pupilDefaultLocalPosition);
            
            SetUpBlinkTween();
            SetUpHitSequence();
            
            _leftEyeDead.gameObject.SetActive(false);
            _rightEyeDead.gameObject.SetActive(false);
            _leftEye.gameObject.SetActive(true);
            _rightEye.gameObject.SetActive(true);
        }
    }
}