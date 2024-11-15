using System;
using System.Collections.Generic;

namespace EngineeredAngel.Database.DI_Container
{
    public class DIContainer
    {
        private readonly Dictionary<Type, Func<object>> _registrations = new();

        public void Register<TService>(Func<TService> factory)
            where TService : class
        {
            _registrations[typeof(TService)] = () => factory();
        }

        public TService Resolve<TService>()
            where TService : class
        {
            if (_registrations.TryGetValue(typeof(TService), out var factory))
            {
                return (TService)factory();
            }

            throw new InvalidOperationException($"Service of type {typeof(TService)} is not registered.");
        }
    }
}
