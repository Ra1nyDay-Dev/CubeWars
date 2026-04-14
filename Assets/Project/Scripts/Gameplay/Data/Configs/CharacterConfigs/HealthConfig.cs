using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs.CharacterConfigs
{
    [CreateAssetMenu(fileName = "HealthConfig", menuName = "Configs/Health")]
    public class HealthConfig : ScriptableObject
    {
        [Range(0, 1000)] public float Max = 100;
        public float Current = 100;
        
        private void OnValidate() => 
            Current = Mathf.Clamp(Current, 0, Max);
    }
}