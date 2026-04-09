using UnityEngine;

namespace Project.Scripts.Infrastructure.Services.Input
{
    public interface IInputService
    {
        Vector2 GetAxis();
        Vector2 GetRelativeAxis();
        bool IsPrimaryAttackButtonDown(); 
        bool IsPrimaryAttackButtonUp(); 
        bool IsSecondaryAttackButtonDown(); 
        bool IsSecondaryAttackButtonUp(); 
        bool IsJumpButtonDown();
        bool IsReloadButtonDown(); 
        bool IsInteractButtonDown();
        GameplayInput GetGameplayInput(Vector3 playerPosition);
    }
}