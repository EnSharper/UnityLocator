using System;
using Ensharp.UnityLocator.Core;
using UnityEngine;

#nullable enable

namespace Ensharp.UnityLocator.LocatorExtends
{
    public static class LocatorComponentExtends
    {
        public static void RegisterSceneLocator<T>(
            this Component component,
            T? service)
            where T : class, ILocatorService
        {
            ThrowIfNullComponent(component);
            component.gameObject.RegisterSceneLocator(service);
        }

        public static void UnregisterSceneLocator<T>(
            this Component component,
            T? service)
            where T : class, ILocatorService
        {
            ThrowIfNullComponent(component);
            component.gameObject.UnregisterSceneLocator(service);
        }

        public static bool IsRegisteredSceneLocator<T>(
            this Component component)
            where T : class, ILocatorService
        {
            ThrowIfNullComponent(component);
            return component.gameObject.IsRegisteredSceneLocator<T>();
        }

        public static T? ResolveSceneLocator<T>(
            this Component component)
            where T : class, ILocatorService
        {
            ThrowIfNullComponent(component);
            return component.gameObject.ResolveSceneLocator<T>();
        }

        public static bool TryResolveSceneLocator<T>(
            this Component component,
            out T? service)
            where T : class, ILocatorService
        {
            ThrowIfNullComponent(component);
            return component.gameObject.TryResolveSceneLocator(out service);
        }

        private static void ThrowIfNullComponent(
            Component component)
        {
            if (component == null) throw new ArgumentNullException(nameof(component));
        }
    }
}
