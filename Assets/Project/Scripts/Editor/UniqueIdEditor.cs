using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UniqueId = Project.Scripts.Gameplay.Common.UniqueId;

namespace Project.Scripts.Editor
{
    [CustomEditor(typeof(UniqueId))]
    public class UniqueIdEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var uniqueId = (UniqueId)target;
            
            if (PrefabUtility.IsPartOfPrefabAsset(uniqueId.gameObject))
                return;
            
            if (string.IsNullOrEmpty(uniqueId.Id))
                Generate(uniqueId);
            else
            {
                UniqueId[] uniqueIds = FindObjectsByType<UniqueId>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                
                if (uniqueIds.Any(other=> other != uniqueId && other.Id == uniqueId.Id))
                    Generate(uniqueId);
            }
        }

        private void Generate(UniqueId uniqueId)
        {
            uniqueId.GenerateId();
            
            if (!Application.IsPlaying(uniqueId))
            {
                EditorUtility.SetDirty(uniqueId);
                EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
            }
        }
    }
}