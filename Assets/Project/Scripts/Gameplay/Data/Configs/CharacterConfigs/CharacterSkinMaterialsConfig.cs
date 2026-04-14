using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs.CharacterConfigs
{
    [CreateAssetMenu(fileName = "CharacterSkinMaterialsConfig", menuName = "Configs/Character/SkinMaterials")]
    public class CharacterSkinMaterialsConfig : ScriptableObject
    {
        public Material PlayerSkinMaterial;
        public Material[] Materials;
    }
}