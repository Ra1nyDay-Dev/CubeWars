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
        // private const string INITIAL_POINT_TAG = "InitialPoint";
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            LevelConfig levelConfig = (LevelConfig)target;

            if (GUILayout.Button("Collect"))
            {
                levelConfig.WeaponSpawners = FindObjectsByType<WeaponSpawnerMarker>(
                        FindObjectsInactive.Exclude,
                        FindObjectsSortMode.None
                    )
                    .Select(x => 
                        new WeaponSpawnerData(
                            x.GetComponent<UniqueId>().Id,
                            x.transform.position,
                            x.WeaponType,
                            x.SpawnTime,
                            x.SpawnOnStart
                        )
                    )
                    .ToList();
                
               levelConfig.SceneName = SceneManager.GetActiveScene().name; 
               
               // levelData.InitialHeroPosition = GameObject.FindWithTag(INITIAL_POINT_TAG).transform.position;
                
            }
            
            EditorUtility.SetDirty(target);
        }
    }
}