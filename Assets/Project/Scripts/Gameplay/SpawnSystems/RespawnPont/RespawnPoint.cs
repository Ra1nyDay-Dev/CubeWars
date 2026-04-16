using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.SpawnSystems.RespawnPont
{
    public class RespawnPoint : MonoBehaviour
    {
        [SerializeField] private float _availableDelayAfterLeavingZone = 3f;

        public bool IsAvailable =>
            _charactersInZone.Count == 0 && _cts == null;

        private readonly List<Character> _charactersInZone = new();
        private CancellationTokenSource _cts;

        public void Construct(RespawnPointData data)
        {
            GetComponent<BoxCollider>()
                .With(col => col.center = data.EnemyCheckZoneCenter)
                .With(col => col.size = data.EnemyCheckZoneSize);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Character character))
            {
                CancelDelay();
                _charactersInZone.Add(character);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Character character))
            {
                _charactersInZone.Remove(character);

                if (_charactersInZone.Count == 0)
                    SetPointAvailableAfterDelay(_availableDelayAfterLeavingZone).Forget();
            }
        }

        private async UniTaskVoid SetPointAvailableAfterDelay(float delay)
        {
            CancelDelay();
            _cts = new CancellationTokenSource();

            try
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(delay),
                    cancellationToken: _cts.Token
                );
            }
            catch (OperationCanceledException) { }
        }
        
        private void CancelDelay()
        {
            if (_cts == null)
                return;

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        private void OnDestroy() => 
            CancelDelay();
    }
}