using UnityEngine;

namespace Project.Scripts.Gameplay.Data.Configs.CharacterConfigs
{
    [CreateAssetMenu(fileName = "CharacterMovementConfig", menuName = "Configs/Character/Movement")]
    public class CharacterMovementConfig : ScriptableObject
    {
        [Range(0,100)] public float MovementSpeed = 10f;
        [Range(0,1000)] public float Acceleration = 100f;
        [Range(0,1000)] public float GroundDeceleration = 50f;
        [Range(0,1000)] public float AirDeceleration = 10f;
        [Range(0,100)] public float RotationSpeed = 20f;
        [Range(-1000,0)] public float Gravity = Physics.gravity.y * 3;
        [Range(-50,0)] public float GroundDownForce = -2f;
        [Range(0,100)] public float JumpHeight = 3f;
    }
}