using Project.Scripts.UI.Services.WindowFactory;
using Project.Scripts.UI.Windows;

namespace Project.Scripts.UI.Services.WindowService
{
    public interface IWindowService
    {
        void Open(WindowId windowId, WindowType windowType = WindowType.Scene);
        void OpenSingle(WindowId windowId, WindowType windowType = WindowType.Scene);
        void Close(WindowId windowId);
        void CloseAll();
    }
}