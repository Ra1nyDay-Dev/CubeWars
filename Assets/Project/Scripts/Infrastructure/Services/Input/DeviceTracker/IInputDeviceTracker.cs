using System;

namespace Project.Scripts.Infrastructure.Services.Input.DeviceTracker
{
    public interface IInputDeviceTracker
    {
        bool AutoSwitchEnabled { get; set; }
        string CurrentControlScheme { get; }

        event Action<string> ControlSchemeChanged;
        
        void SwitchControlScheme(ControlSchemeType schemeType);
    }
}