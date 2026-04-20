using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Gameplay.CharacterSystems;
using Project.Scripts.Gameplay.CharacterSystems.Brain;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Configs.CharacterConfigs;
using Project.Scripts.Gameplay.Services.CameraProvider;
using Project.Scripts.Gameplay.Services.Factories.BrainFactory;
using Project.Scripts.Gameplay.Services.Factories.CharacterFactory;
using Project.Scripts.Gameplay.Services.Factories.RespawnPointFactory;
using Project.Scripts.Gameplay.Services.Factories.WeaponSpawnerFactory;
using Project.Scripts.Gameplay.Services.RespawnService;
using Project.Scripts.Infrastructure.Data;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using UnityEngine;

namespace Project.Scripts.Gameplay.Services.Factories.LevelFactory
{
    public class PolygonLevelFactory : DeathmatchLevelFactory
    {
        public PolygonLevelFactory(
            ICameraProvider cameraProvider,
            IConfigProvider configProvider,
            IWeaponSpawnerFactory weaponSpawnerFactory,
            IRespawnPointFactory respawnPointFactory,
            ICharacterFactory characterFactory,
            IBrainFactory brainFactory,
            IRespawnService respawnService) 
            : base(
                cameraProvider,
                configProvider,
                weaponSpawnerFactory,
                respawnPointFactory,
                characterFactory,
                brainFactory,
                respawnService)
        {
        }

        protected override void CreateBotsCharacters(Queue<InitialPointData> initialPoints, int botsCount)
        {
            CharacterSkinMaterialsConfig skinsConfig = _configProvider.GetCharacterSkinMaterialsConfig();
            Queue<Material> botsMaterials = new(
                skinsConfig.Materials
                    .ToList()
                    .Shuffle()
                    .Where(x => x != skinsConfig.PlayerSkinMaterial));
            
            for (int i = 0; i < botsCount; i++)
            {
                InitialPointData initialPointData = initialPoints.Dequeue();
                Material material = botsMaterials.Dequeue();
                
                Character emptyCharacter = _characterFactory.Create(initialPointData.Position, initialPointData.Rotation, material);
                _brainFactory.Create(emptyCharacter, BrainType.Empty);
                
                botsMaterials.Enqueue(material);
            }
        }
    }
    
}