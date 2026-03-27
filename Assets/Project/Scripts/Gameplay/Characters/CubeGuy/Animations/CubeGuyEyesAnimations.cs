using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Gameplay.Characters.CubeGuy.Animations
{
    public class CubeGuyEyesAnimations : MonoBehaviour
    {
        [SerializeField] private Transform _leftPupil;
        [SerializeField] private Transform _rightPupil;

        [SerializeField] private float _minBlinkInterval = 1.5f;
        [SerializeField] private float _maxBlinkInterval = 3f;
        [SerializeField] private float _minPupilChangePositionInterval = 0.3f;
        [SerializeField] private float _maxPupilChangePositionInterval = 3f;
        [SerializeField] private List<Vector3> _pupilPositions = new List<Vector3>();
        
        private readonly int _blink = Animator.StringToHash("Blink");
        
        private Animator _eyesAnimator;
        private Vector3 _pupilDefaultLocalPosition;
        private Coroutine _currentPupilsState;
        private Coroutine _currentBlinkState;

        private void Awake()
        {
            _eyesAnimator = GetComponent<Animator>();
            _pupilDefaultLocalPosition = _leftPupil.localPosition;
        }

        private void Start() => 
            SetIdleAnimation();

        public void Blink() => 
            _eyesAnimator.SetTrigger(_blink);

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
                SetPupilsPositions(pupilPosition);
            }
        }
    }
}