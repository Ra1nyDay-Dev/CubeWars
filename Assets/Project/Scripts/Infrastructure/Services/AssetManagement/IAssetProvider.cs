using UnityEngine;

namespace Project.Scripts.Infrastructure.Services.AssetManagement
{
    public interface IAssetProvider 
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 place);
        GameObject Instantiate(string path, Vector3 place, Quaternion rotation);
        GameObject Instantiate(GameObject prefab);
        GameObject Instantiate(string path, Vector3 place, Quaternion rotation, Transform parent);
    }
}