using System;
using Project.Scripts.Gameplay.Data.Enums;

namespace Project.Scripts.Gameplay.Data.Configs.AI
{
    [Serializable]
    public struct WeaponPriority
    {
        public WeaponType WeaponType;
        public int Priority;
    }
}