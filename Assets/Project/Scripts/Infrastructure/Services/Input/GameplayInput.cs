using UnityEngine;

namespace Project.Scripts.Infrastructure.Services.Input
{
    public struct GameplayInput
    {
        public Vector2 Move;
        public Vector3 Look;
        public bool Jump;
        public bool StartPrimaryFire;
        public bool StopPrimaryFire;
        public bool StartSecondaryFire;
        public bool StopSecondaryFire;
        public bool Reload;
        public bool Interact;
    }
}