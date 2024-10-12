using System;
using Ensharp.UnityLocator.Core;
using UnityEngine;

namespace Ensharp.UnityLocator.LocatorExtends
{
    public static class LocatorComponentExtends
    {
        public static void RegisterSceneLocator<T>(this Component component, T service)
            where T : class, ILocatorService
        {
            ThrowIfNullComponent(component);
            component.gameObject.RegisterSceneLocator(service);
        }

        public static void UnRegisterSceneLocator<T>(this Component component, T service)
            where T : class, ILocatorService
        {
            ThrowIfNullComponent(component);
            component.gameObject.UnRegisterSceneLocator(service);
        }

        public static bool IsRegisteredSceneLocator<T>(this Component component)
            where T : class, ILocatorService
        {
            ThrowIfNullComponent(component);
            return component.gameObject.IsRegisteredSceneLocator<T>();
        }

        public static T GetSceneLocator<T>(this Component component)
            where T : class, ILocatorService
        {
            ThrowIfNullComponent(component);
            return component.gameObject.GetSceneLocator<T>();
        }

        public static bool TryGetSceneLocator<T>(this Component component, out T service)
            where T : class, ILocatorService
        {
            ThrowIfNullComponent(component);
            return component.gameObject.TryGetSceneLocator(out service);
        }

        private static void ThrowIfNullComponent(Component component)
        {
            if (component == null) throw new ArgumentNullException(nameof(component));
        }
    }
}
