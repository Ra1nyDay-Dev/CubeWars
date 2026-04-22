using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs.LevelConfigs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level")]
    public class LevelConfig : ScriptableObject
    {
        public string MapName;
        public string SceneName;
        public Sprite MapImage;
        
        [Range(0, 300)] public float RespawnTimeSeconds;
        
        [Header("Level objects")]
        public List<WeaponSpawnerData> WeaponSpawners;
        public List<InitialPointData> InitialPoints;
        public List<RespawnPointData> RespawnPoints;
    }
}