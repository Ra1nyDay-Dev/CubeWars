using System;
using UnityEngine;

namespace Project.Scripts.Infrastructure.Services.Input.ActionMaps.Gameplay
{
    public interface IGameplayActions : InputActions.IGameplayActions
    {
        // properties to use pull-driven input system in updates
        Vector2 MoveInput { get; }
        Vector2 LookInput { get; }
        bool IsPrimaryButtonDown { get; }
        bool IsPrimaryButtonUp { get; }
        bool IsSecondaryButtonDown { get; }
        bool IsSecondaryButtonUp { get; }
        bool IsReloadButtonDown { get; }
        bool IsInteractButtonDown { get; }
        bool IsJumpButtonDown { get; }
        
        // Events to use even-driven input system
        event Action<Vector2> MoveInputChanged;
        event Action<Vector2> LookInputChanged;
        event Action PrimaryButtonDown;
        event Action PrimaryButtonUp;
        event Action SecondaryButtonDown;
        event Action SecondaryButtonUp;
        event Action ReloadButtonDown;
        event Action InteractButtonDown;
        event Action JumpButtonDown;
        
        Vector2 GetRelativeMoveInput(Camera camera);
        Vector3 GetRelativeLookInput(Vector3 playerPosition, Camera camera);
    }
}