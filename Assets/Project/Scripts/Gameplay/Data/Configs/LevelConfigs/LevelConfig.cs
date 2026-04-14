using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs.LevelConfigs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level")]
    public class LevelConfig : ScriptableObject
    {
        public string SceneName;
        public List<WeaponSpawnerData> WeaponSpawners;
        public List<InitialPointData> InitialPoints;
    }
}