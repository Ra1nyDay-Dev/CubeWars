using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Gameplay.Services.CameraProvider
{
  public class CinemachineCameraProvider : ICameraProvider
  { 
      public Camera Camera { get; private set; }
      
      private CinemachineCamera _cinemachineCamera;

      [Inject]
      public CinemachineCameraProvider(
          Camera camera,
          CinemachineCamera cinemachineCamera)
      {
          Camera = camera;
          _cinemachineCamera = cinemachineCamera;
      }

      public void SetFollowTarget(Transform target) =>
          _cinemachineCamera.Target.TrackingTarget = target;
  }
}