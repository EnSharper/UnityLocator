using System;
using Ensharp.UnityLocator.Core;
using UnityEngine;

namespace Ensharp.UnityLocator.LocatorExtends
{
    public static class LocatorGameObjectExtends
    {
        public static void RegisterSceneLocator<T>(this GameObject gameObject, T service)
            where T : class, ILocatorService
        {
            ThrowIfNullGameObject(gameObject);
            SceneLocator.Register(gameObject.scene, service);
        }

        public static void UnRegisterSceneLocator<T>(this GameObject gameObject, T service)
            where T : class, ILocatorService
        {
            ThrowIfNullGameObject(gameObject);
            SceneLocator.Unregister(gameObject.scene, service);
        }

        public static bool IsRegisteredSceneLocator<T>(
            this GameObject gameObject)
            where T : class, ILocatorService
        {
            ThrowIfNullGameObject(gameObject);
            return SceneLocator.IsRegistered<T>(gameObject.scene);
        }

        public static T GetSceneLocator<T>(this GameObject gameObject)
            where T : class, ILocatorService
        {
            ThrowIfNullGameObject(gameObject);
            return SceneLocator.Get<T>(gameObject.scene);
        }

        public static bool TryGetSceneLocator<T>(this GameObject gameObject, out T service)
            where T : class, ILocatorService
        {
            ThrowIfNullGameObject(gameObject);
            return SceneLocator.TryGet(gameObject.scene, out service);
        }

        private static void ThrowIfNullGameObject(GameObject gameObject)
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
        }
    }
}
