using System;
using System.Collections.Generic;

namespace Project.Scripts.Infrastructure.Services.ServiceLocator
{
    public abstract class ServiceLocator<TChildLocator, TChildInterface>
        where TChildLocator : ServiceLocator<TChildLocator, TChildInterface>, new()
        where TChildInterface : class, IService
    {
        public static TChildLocator Container => Instance;

        protected static TChildLocator Instance => _instance ??= new TChildLocator();
        protected static readonly Dictionary<(string, Type), TChildInterface> _services = new();

        private static TChildLocator _instance;

        protected ServiceLocator()
        {
        }

        public void Register<TService>(TService service) where TService : class, TChildInterface
        {
            Register<TService>(null, service);
        }

        public void Register<TService>(string tag, TService service) where TService : class, TChildInterface
        {
            var key = (tag, typeof(TService));

            if (!_services.TryAdd(key, service))
                throw new InvalidOperationException($"Service {typeof(TService).Name} already registered");
        }

        public TService Get<TService>(string tag = null) where TService : class, TChildInterface
        {
            var key = (tag, typeof(TService));

            if (_services.TryGetValue(key, out TChildInterface service))
                return service as TService;

            throw new InvalidOperationException($"Service {typeof(TService).Name} not registered");
        }

        public bool TryGet<T>(out T service, string tag = null) where T : class, TChildInterface
        {
            var key = (tag, typeof(T));
            service = null;

            if (_services.TryGetValue(key, out TChildInterface obj))
            {
                service = obj as T;
                return service != null;
            }

            return false;
        }
    }
}

    