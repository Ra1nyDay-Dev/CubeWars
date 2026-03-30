using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.Gameplay.Characters.HealthSystems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Gameplay.Characters.CubeGuy.Animations
{
    public class CubeGuyEyesAnimations : MonoBehaviour
    {
        [SerializeField] private Transform _leftEye;
        [SerializeField] private Transform _leftPupil;
        [SerializeField] private Transform _rightEye;
        [SerializeField] private Transform _rightPupil;

        [SerializeField] private float _minBlinkInterval = 1.5f;
        [SerializeField] private float _maxBlinkInterval = 3f;
        [SerializeField] private float _minPupilChangePositionInterval = 0.3f;
        [SerializeField] private float _maxPupilChangePositionInterval = 3f;
        [SerializeField] private List<Vector3> _pupilPositions = new List<Vector3>();
        
        private Character _character;
        private IDamageable _damageable;
        private Vector3 _pupilDefaultLocalPosition;
        
        private Coroutine _currentPupilsState;
        private Coroutine _currentBlinkState;
        private Tween _blinkTween;
        private Tween _pupilTween;
        private Sequence _hitSequence;

        private void Awake()
        {
            _character = GetComponentInParent<Character>();
            _damageable = _character.GetComponent<IDamageable>();
            _pupilDefaultLocalPosition = _leftPupil.localPosition;

            SetUpBlinkTween();
            SetUpHitSequence();
        }

        private void Start() => 
            SetIdleAnimation();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
                OnHit();
        }
        
        private void OnEnable()
        {
            _damageable.Damaged += OnHit;
        }

        private void OnDisable()
        {
            _damageable.Damaged -= OnHit;
        }

        private void OnDestroy()
        {
            _blinkTween.Kill();
            _pupilTween.Kill();
            _hitSequence.Kill();
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
                .SetAutoKill(false)
                .OnComplete(SetIdleAnimation);

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

        public void Blink() => 
            _blinkTween.Restart();

        public void SetIdleAnimation()
        {
            if (_currentPupilsState != null)
                StopCoroutine(_currentPupilsState);
            
            if (_currentBlinkState != null)
                StopCoroutine(_currentBlinkState);
            
            _currentPupilsState = StartCoroutine(PupilsAnimation());
            _currentBlinkState = StartCoroutine(BlinkAnimation());
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

        private void OnHit()
        {
            StopAllCoroutines();
            MovePupils(_pupilDefaultLocalPosition);
            _hitSequence.Restart();
        }
    }
}