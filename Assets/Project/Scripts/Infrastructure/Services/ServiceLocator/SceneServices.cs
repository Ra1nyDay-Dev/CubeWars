using System;

namespace Project.Scripts.Infrastructure.Services.ServiceLocator
{
    public class SceneServices : ServiceLocator<SceneServices, ISceneService>
    {
        public static void Dispose()
        {
            foreach (var service in _services.Values)
            {
                if (service is IDisposable disposable)
                    disposable.Dispose();
            }
                
            _services.Clear();
        }
    }
}