using System.Collections.Generic;
using Project.Scripts.UI.Services.WindowFactory;
using Project.Scripts.UI.Windows;
using UnityEngine;

namespace Project.Scripts.UI.Services.WindowService
{
  public class WindowService : IWindowService
  {
    private readonly IWindowFactory _windowFactory;

    private readonly List<BaseWindow> _openedWindows = new();

    public WindowService(IWindowFactory windowFactory) =>
      _windowFactory = windowFactory;
    
    public void Open(WindowId windowId, WindowType windowType = WindowType.Scene) =>
      _openedWindows.Add(_windowFactory.CreateWindow(windowId, windowType));
    
    public void OpenSingle(WindowId windowId, WindowType windowType = WindowType.Scene)
    {
      CloseAll();
      Open(windowId, windowType);
    }

    public void Close(WindowId windowId)
    {
      BaseWindow window = _openedWindows.Find(x => x.Id == windowId);
      
      _openedWindows.Remove(window);
      
      if (window != null)
        Object.Destroy(window.gameObject);
    }
    
    public void CloseAll() 
    {
      foreach (BaseWindow window in _openedWindows)
        Close(window.Id);
    }
  }
}