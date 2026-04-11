using System;
using Project.Scripts.Infrastructure.Services.Input.ActionMaps;
using Project.Scripts.Infrastructure.Services.Input.ActionMaps.Gameplay;
using UnityEngine;

namespace Project.Scripts.Infrastructure.Services.Input
{
    public interface IInputService
    {
        IGameplayActions GameplayActions { get; }
        ActionMapType CurrentActionMap { get; }
        void SwitchActionMap(ActionMapType actionMapType);
    }
}