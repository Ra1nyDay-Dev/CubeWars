using System.IO;
using UnityEngine;

namespace Project.Scripts.Infrastructure.Services.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject LoadAsset(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);

            if (prefab == null)
                throw new FileNotFoundException($"Cant find prefab at path {path}");

            return prefab;
        }
        
        
        public T LoadAsset<T>(string path) where T : Component
        {
            T prefab = Resources.Load<T>(path);
            
            if (prefab == null)
                throw new FileNotFoundException($"Cant find prefab with type {typeof(T)} at path {path}");
            
            return prefab;
        }
    }
}