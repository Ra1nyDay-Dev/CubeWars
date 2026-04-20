using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Editor
{
    public class MaterialReplacerWindow : EditorWindow
    {
        [System.Serializable]
        public class ReplacePair
        {
            public Material from;
            public Material to;
        }

        private List<ReplacePair> pairs = new List<ReplacePair>();

        [MenuItem("Tools/Material Replacer")]
        public static void ShowWindow()
        {
            GetWindow<MaterialReplacerWindow>("Material Replacer");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Material Replacement Tool", EditorStyles.boldLabel);

            if (GUILayout.Button("Add Pair"))
            {
                pairs.Add(new ReplacePair());
            }

            for (int i = 0; i < pairs.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                pairs[i].from = (Material)EditorGUILayout.ObjectField(pairs[i].from, typeof(Material), false);
                GUILayout.Label("→", GUILayout.Width(20));
                pairs[i].to = (Material)EditorGUILayout.ObjectField(pairs[i].to, typeof(Material), false);

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    pairs.RemoveAt(i);
                    i--;
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Replace In Scene"))
            {
                ReplaceMaterials();
            }
        }

        private void ReplaceMaterials()
        {
            var renderers = FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            int count = 0;

            foreach (var rend in renderers)
            {
                var mats = rend.sharedMaterials;
                bool changed = false;

                for (int i = 0; i < mats.Length; i++)
                {
                    foreach (var pair in pairs)
                    {
                        if (pair.from != null && mats[i] == pair.from)
                        {
                            mats[i] = pair.to;
                            changed = true;
                            count++;
                        }
                    }
                }

                if (changed)
                {
                    rend.sharedMaterials = mats;
                    EditorUtility.SetDirty(rend);
                }
            }

            Debug.Log($"Replaced {count} materials");
        }
    }
}
