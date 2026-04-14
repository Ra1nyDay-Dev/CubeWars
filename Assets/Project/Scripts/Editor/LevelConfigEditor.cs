using System.Linq;
using Project.Scripts.Gameplay.Common;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Gameplay.Data.Configs.LevelConfigs;
using Project.Scripts.Gameplay.Weapons.WeaponSpawn;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Editor
{
    [CustomEditor(typeof(LevelConfig))]
    public class LevelConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            LevelConfig levelConfig = (LevelConfig)target;

            if (GUILayout.Button("Collect"))
            {
                levelConfig.SceneName = SceneManager.GetActiveScene().name; 
                CollectWeaponSpawnersData(levelConfig);
                CollectInitialPointsData(levelConfig);
            }
            
            EditorUtility.SetDirty(target);
        }

        private static void CollectWeaponSpawnersData(LevelConfig levelConfig)
        {
            levelConfig.WeaponSpawners = FindObjectsByType<WeaponSpawnerMarker>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None
                )
                .Select(x => 
                    new WeaponSpawnerData(
                        x.transform.position,
                        x.WeaponType,
                        x.SpawnTime,
                        x.SpawnOnStart
                    )
                )
                .ToList();
        }

        private void CollectInitialPointsData(LevelConfig levelConfig)
        {
            levelConfig.InitialPoints = FindObjectsByType<InitialPointMarker>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.InstanceID
                )
                .Select(x => 
                    new InitialPointData(
                        x.transform.position,
                        x.transform.rotation
                    )
                )
                .ToList();
        }
    }
}