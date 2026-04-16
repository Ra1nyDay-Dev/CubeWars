using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Infrastructure.Data;
using UnityEngine;

namespace Project.Scripts.Gameplay.SpawnSystems.RespawnPont
{
    public class RespawnPoint : MonoBehaviour
    {
        [SerializeField] private LayerMask _charactersLayerMask;

        private const double DELAY_BETWEEN_CHARACTERS_CHECKS = 1f;
        
        public bool IsAvailable =>
            _isAvailable;
        
        private bool _isAvailable;

        private BoxCollider _collider;
        private readonly Collider[] _overlapResults = new Collider[1];

        public void Construct(RespawnPointData data)
        {
            _collider = GetComponent<BoxCollider>();
            _collider.center = data.EnemyCheckZoneCenter;
            _collider.size = data.EnemyCheckZoneSize;
            CharactersCheckLoop(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask CharactersCheckLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _isAvailable = !HasAnyCharacterInside();
                await UniTask.Delay(TimeSpan.FromSeconds(DELAY_BETWEEN_CHARACTERS_CHECKS),
                    cancellationToken: CancellationToken.None);
            }
        } 

        private bool HasAnyCharacterInside()
        {
            int overlapResultsCount = Physics.OverlapBoxNonAlloc(
                transform.position + _collider.center,
                _collider.size / 2,
                _overlapResults,
                transform.rotation,
                _charactersLayerMask);

            if (overlapResultsCount > 0)
            {
                foreach (var hit in _overlapResults)
                {
                    if (hit.TryGetComponent(out Character character))
                        return true;
                }
            }
            return false;
        }
        
#if UNITY_EDITOR       
        private void OnDrawGizmos()
        {
            if (_collider == null)
                return;
            
            Color32 availableColor = new Color32(020, 249, 49, 130);
            Color32 notAvailableColor = new Color32(220, 49, 49, 130);
            
            Gizmos.color = IsAvailable ? availableColor : notAvailableColor;
            Gizmos.DrawCube(transform.position + _collider.center, _collider.size);
        }
#endif
    }
}