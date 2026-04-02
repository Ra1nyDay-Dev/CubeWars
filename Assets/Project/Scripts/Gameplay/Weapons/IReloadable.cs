using UnityEngine;

namespace Project.Scripts.Gameplay.Weapons
{
    public interface IReloadable
    {
        Awaitable Reload();
    }
}