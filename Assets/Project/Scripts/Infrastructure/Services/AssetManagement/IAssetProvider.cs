using UnityEngine;

namespace Project.Scripts.Infrastructure.Services.AssetManagement
{
    public interface IAssetProvider 
    {
        GameObject LoadAsset(string path);
        T LoadAsset<T>(string path) where T : Component;
    }
}