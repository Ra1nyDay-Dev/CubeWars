using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Services.Factories.CharacterFactory;
using Project.Scripts.Gameplay.Services.Factories.RespawnPointFactory;
using Project.Scripts.Gameplay.SpawnSystems.RespawnPont;
using Project.Scripts.Infrastructure.Data;
using Zenject;

namespace Project.Scripts.Gameplay.Services.RespawnService
{
    public class RespawnService : IRespawnService, IDisposable
    {
        private const double DELAY_BETWEEN_ZONE_CHECKS = 1f;
        
        private readonly IRespawnPointFactory _respawnPointFactory;
        private readonly ICharacterFactory _characterFactory;
        
        private List<Character> _characters;
        private readonly Dictionary<Character, Action<DamageData>> _eventHandlers;
        private readonly Dictionary<Character, CancellationTokenSource> _pendingRespawns;
        private float _respawnTime;

        [Inject]
        public RespawnService(
            IRespawnPointFactory respawnPointFactory,
            ICharacterFactory characterFactory)
        {
            _respawnPointFactory = respawnPointFactory;
            _characterFactory = characterFactory;

            _characters = new List<Character>();
            _eventHandlers = new Dictionary<Character, Action<DamageData>>();
            _pendingRespawns = new Dictionary<Character, CancellationTokenSource>();
        }

        public bool TryGetAvailableRespawnPoint(out RespawnPoint respawnPoint)
        {
            respawnPoint = _respawnPointFactory.RepawnPoints
                .Shuffle()
                .FirstOrDefault(point => point.IsAvailable);
            
            return respawnPoint != null;
        }

        public void Initialize(float levelRespawnTime)
        {
           _respawnTime = levelRespawnTime;
           
           if (_respawnPointFactory.RepawnPoints.Count == 0)
               throw new InvalidOperationException("Respawn points is not created.");
           
           _characters = new List<Character>(_characterFactory.Characters);
           
           if (_characters.Count == 0)
               throw new InvalidOperationException("No characters created.");
           
           SubscribeToEvents();
        }

        public void Dispose()
        {
            UnsubscribeFromEvents();
            _characters.Clear();
            _eventHandlers.Clear();

            foreach (CancellationTokenSource cts in _pendingRespawns.Values)
            {
                cts.Cancel();
                cts.Dispose();
            }
            
            _pendingRespawns.Clear();
        }

        private void OnCharacterDied(Character character, DamageData damageData)
        {
            CancelRespawn(character);

            var cts = new CancellationTokenSource();
            _pendingRespawns[character] = cts;
            
            WaitAndRespawn(character, _respawnTime, cts.Token).Forget();
        }
        
        private async UniTaskVoid WaitAndRespawn(Character character, float respawnTime, CancellationToken cancellationToken)
        {
            try
            {
                await WaitRespawnTime(respawnTime, cancellationToken);

                RespawnPoint respawnPoint = await WaitForRespawnPoint(cancellationToken);
                character.gameObject.transform.position = respawnPoint.gameObject.transform.position;
                character.gameObject.transform.rotation = respawnPoint.gameObject.transform.rotation;
                character.RespawnBehaviour.Respawn();
            }
            catch (OperationCanceledException) { }
            finally
            {
                _pendingRespawns.Remove(character);
            }
        }

        private static UniTask WaitRespawnTime(float respawnTime, CancellationToken cancellationToken) => 
            UniTask.Delay(TimeSpan.FromSeconds(respawnTime), cancellationToken: cancellationToken);

        private async UniTask<RespawnPoint> WaitForRespawnPoint(CancellationToken token)
        {
            RespawnPoint point = null;

            while (point == null && _respawnPointFactory.RepawnPoints.Count > 0)
            {
                TryGetAvailableRespawnPoint(out point);

                if (point == null)
                    await UniTask.Delay(TimeSpan.FromSeconds(DELAY_BETWEEN_ZONE_CHECKS), cancellationToken: token);
            }

            return point;
        }

        private void SubscribeToEvents()
        {
            foreach (var character in _characters)
            {
                Action<DamageData> handler = data => OnCharacterDied(character, data);
                _eventHandlers[character] = handler;
                character.RespawnBehaviour.Dead += handler;
            }
        }

        private void UnsubscribeFromEvents()
        {
            foreach (var character in _characters)
            {
                if (_eventHandlers.TryGetValue(character, out var handler))
                    character.RespawnBehaviour.Dead -= handler;
            }
        }
        
        private void CancelRespawn(Character character)
        {
            if (_pendingRespawns.TryGetValue(character, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
                _pendingRespawns.Remove(character);
            }
        }
    }
}