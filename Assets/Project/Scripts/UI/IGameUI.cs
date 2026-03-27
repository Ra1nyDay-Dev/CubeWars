using Project.Scripts.Infrastructure.Services.ServiceLocator;
using UnityEngine;

namespace Project.Scripts.UI
{
    public interface IGameUI : IProjectService
    {
        void AttachSceneUI(GameObject sceneUI);
    }
}