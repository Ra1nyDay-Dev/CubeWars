using System;
using Project.Scripts.UI.Services.WindowFactory;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI
{
    public abstract class SceneUI : MonoBehaviour, IInitializable
    {
        protected IWindowFactory _windowFactory;

        [Inject]
        public virtual void Construct(IWindowFactory windowFactory) => 
            _windowFactory = windowFactory;

        public virtual void Initialize() => 
            _windowFactory.AttachSceneUI(this);
    }
}