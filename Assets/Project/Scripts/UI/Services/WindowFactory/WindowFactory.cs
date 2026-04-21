using System;
using Project.Scripts.Infrastructure.Services.ConfigProvider;
using Project.Scripts.UI.Windows;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Project.Scripts.UI.Services.WindowFactory
{
  public class WindowFactory : IWindowFactory
  {
    private readonly IConfigProvider _configProvider;
    private readonly IInstantiator _instantiator;
    private readonly Transform _gameUiRoot;
    private readonly Transform _sceneUiRoot;
    private SceneUI _currentSceneUI;

    public WindowFactory(
      IConfigProvider configProvider, 
      IInstantiator instantiator,
      Transform gameUiRoot,
      Transform sceneUiRoot)
    {
      _configProvider = configProvider;
      _instantiator = instantiator;
      _gameUiRoot = gameUiRoot;
      _sceneUiRoot = sceneUiRoot;
    }

    public void AttachSceneUI(SceneUI sceneUI)
    {
      ClearSceneUI();
      sceneUI.transform.SetParent(_sceneUiRoot, false);
      FixUiTransform(sceneUI);
      _currentSceneUI = sceneUI;
    }

    public void ClearSceneUI()
    {
      for (int i = _sceneUiRoot.childCount - 1; i >= 0; i--)
        Object.Destroy(_sceneUiRoot.GetChild(i).gameObject);
    }

    public BaseWindow CreateWindow(WindowId windowId, WindowType windowType = WindowType.Scene)
    {
      if (windowType == WindowType.Scene && _currentSceneUI == null)
        throw new NullReferenceException("Scene UI is not attached");
      
      Transform root = windowType == WindowType.Scene ? _currentSceneUI.transform : _gameUiRoot;
      
      return _instantiator.InstantiatePrefabForComponent<BaseWindow>(_configProvider.GetWindowPrefab(windowId), root);
    }

    private void FixUiTransform(SceneUI sceneUI)
    {
      var rt = sceneUI.GetComponent<RectTransform>();
      rt.anchorMin = Vector2.zero;
      rt.anchorMax = Vector2.one;
      rt.offsetMin = Vector2.zero;
      rt.offsetMax = Vector2.zero;
      sceneUI.transform.localScale = Vector3.one;
    }
  }
}