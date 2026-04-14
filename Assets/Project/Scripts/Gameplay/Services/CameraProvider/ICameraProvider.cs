using UnityEngine;

namespace Project.Scripts.Gameplay.Services.CameraProvider
{
    public interface ICameraProvider
    {
        Camera Camera { get; }
        void SetFollowTarget(Transform target);
    }
}