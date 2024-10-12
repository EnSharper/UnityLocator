// #define ENABLE_LOG

// ReSharper disable SuspiciousTypeConversion.Global
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ensharp.UnityLocator.Core
{
    /// <summary>
    /// Locatorを管理するコンテナ
    /// </summary>
    public class LocatorContainer : ILocatorContainer
    {
        private readonly Dictionary<Type, ILocatorService> _services = new();

        /// <inheritdoc/>
        public void Register<TLocatorService>(TLocatorService locatorService)
            where TLocatorService : class, ILocatorService
        {
            if (locatorService == null)
            {
                Log($"Please not null value : {typeof(TLocatorService)}");
                return;
            }

            var type = typeof(TLocatorService);
            if (!_services.TryAdd(type, locatorService))
            {
                Log($"LocatorService already registered {typeof(TLocatorService)}");
            }
        }

        /// <inheritdoc/>
        public void Unregister<TLocatorService>(TLocatorService service)
            where TLocatorService : class, ILocatorService
        {
            var registeredService = _services.FirstOrDefault(pair => Equals(pair.Value, service));
            if (registeredService.Key == null)
            {
                LogWarn($"The passed service is not registered: {service.GetType()}");
                return;
            }
            
            if (registeredService.Value is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _services.Remove(registeredService.Key);
        }

        /// <inheritdoc/>
        public bool IsRegistered<TLocatorService>()
            where TLocatorService : class, ILocatorService
        {
            var type = typeof(TLocatorService);

            return _services.ContainsKey(type);
        }

        /// <inheritdoc/>
        public TLocatorService Resolve<TLocatorService>()
            where TLocatorService : class, ILocatorService
        {
            var type = typeof(TLocatorService);
            if (_services.TryGetValue(type, out var service))
            {
                return service as TLocatorService;
            }

            LogError($"A service of the given type is not registered: {type.Name}");
            return null;
        }

        /// <inheritdoc/>
        public bool TryResolve<TLocatorService>(out TLocatorService service)
            where TLocatorService : class, ILocatorService
        {
            var type = typeof(TLocatorService);
            if (_services.TryGetValue(type, out var registeredService))
            {
                service = registeredService as TLocatorService;
                return true;
            }
            
            service = null;
            return false;
        }

        /// <inheritdoc/>
        public void DisposeRegisteredServices()
        {
            foreach (var (_, value) in _services)
            {
                if (value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _services.Clear();
        }

        [System.Diagnostics.ConditionalAttribute("ENABLE_LOG")]
        private void Log(string message)
        {
            UnityEngine.Debug.Log(message);
        }

        [System.Diagnostics.ConditionalAttribute("ENABLE_LOG")]
        private void LogWarn(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        
        [System.Diagnostics.ConditionalAttribute("ENABLE_LOG")]
        private void LogError(string message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}
