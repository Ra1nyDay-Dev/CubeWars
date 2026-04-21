using System;
using Project.Scripts.UI.Elements;
using Project.Scripts.UI.Services.LoadingScreen;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI
{
    public class GameUI : MonoBehaviour
    {
        [field: SerializeField] public Transform SceneUiRoot { get; private set; }
        [field: SerializeField] public Transform GameUIRoot { get; private set; }
        [field: SerializeField] public LoadingScreen LoadingScreen { get; private set; }

        private void Awake() => 
            DontDestroyOnLoad(gameObject);
    }
}
