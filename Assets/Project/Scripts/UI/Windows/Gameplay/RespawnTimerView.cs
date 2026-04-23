using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.Windows.Gameplay
{
    public class RespawnTimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        
        private float _secondsLeft;
        
        private CancellationTokenSource _cancellationTokenSource;
        
        public void StartCountdown(float seconds)
        {
            _secondsLeft = seconds;
                
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            
            RunCountdownTask(_cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid RunCountdownTask(CancellationToken token)
        {
            while (_secondsLeft > 0)
            {
                _timerText.text = $"You will respawn in {Mathf.CeilToInt(_secondsLeft)} seconds";
                await UniTask.WaitForSeconds(1, cancellationToken: token);
                _secondsLeft -= 1f;
            }

            _timerText.text = "Waiting for a free respawn point";
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}