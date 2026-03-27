using System.Collections;
using Project.Scripts.Infrastructure.Services.ServiceLocator;
using UnityEngine;

namespace Project.Scripts.Infrastructure.Services.CoroutineRunner
{
    public interface ICoroutineRunner : IProjectService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}