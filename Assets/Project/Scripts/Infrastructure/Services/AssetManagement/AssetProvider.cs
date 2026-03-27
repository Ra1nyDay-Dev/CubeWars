using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.Scripts.Infrastructure.Services.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Instantiate(string path)
        {
            var prefab = LoadPrefab(path);
            return Object.Instantiate(prefab);
        }

        public GameObject Instantiate(string path, Vector3 place)
        {
            var prefab = LoadPrefab(path);
            return Object.Instantiate(prefab, place, Quaternion.identity);
        }
        
        public GameObject Instantiate(string path, Vector3 place, Quaternion rotation)
        {
            var prefab = LoadPrefab(path);
            return Object.Instantiate(prefab, place, rotation);
        }

        private GameObject LoadPrefab(string path)
        {
            var prefab = Resources.Load<GameObject>(path);

            if (prefab == null)
                throw new Exception("Cant find prefab at path " + path);

            return prefab;
        }
    }
}