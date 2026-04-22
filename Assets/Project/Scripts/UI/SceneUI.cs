using System;
using Project.Scripts.UI.Services.WindowFactory;
using Project.Scripts.UI.Services.WindowService;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI
{
    public abstract class SceneUI : MonoBehaviour, IInitializable, IDisposable
    {
        protected IWindowFactory _windowFactory;
        private IWindowService _windowService;

        [Inject]
        public virtual void Construct(IWindowFactory windowFactory, IWindowService windowService)
        {
            _windowFactory = windowFactory;
            _windowService = windowService;
        }

        public virtual void Initialize() => 
            _windowFactory.AttachSceneUI(this);

        public void Dispose() => 
            _windowService.CloseAll();
    }
}