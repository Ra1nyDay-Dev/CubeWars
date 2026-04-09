using System.IO;
using Project.Scripts.Gameplay.Data;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure.Services.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private DiContainer _diContainer;
        
        [Inject]
        public void Construct(DiContainer diContainer) => 
            _diContainer = diContainer;

        public GameObject Instantiate(string path) => 
            _diContainer.InstantiatePrefab(LoadPrefab(path));

        public GameObject Instantiate(GameObject prefab) => 
            _diContainer.InstantiatePrefab(prefab);

        public GameObject Instantiate(string path, Vector3 place) =>
            _diContainer.InstantiatePrefab(LoadPrefab(path))
                .With(go => go.transform.position = place )
                .With(go => go.transform.rotation = Quaternion.identity);

        public GameObject Instantiate(string path, Vector3 place, Quaternion rotation) =>
            _diContainer.InstantiatePrefab(LoadPrefab(path))
                .With(go => go.transform.position = place )
                .With(go => go.transform.rotation = rotation);
        
        public GameObject Instantiate(string path, Vector3 place, Quaternion rotation, Transform parent) =>
            _diContainer.InstantiatePrefab(LoadPrefab(path), place, rotation, parent);

        private GameObject LoadPrefab(string path)
        {
            var prefab = Resources.Load<GameObject>(path);

            if (prefab == null)
                throw new FileNotFoundException("Cant find prefab at path " + path);

            return prefab;
        }
    }
}