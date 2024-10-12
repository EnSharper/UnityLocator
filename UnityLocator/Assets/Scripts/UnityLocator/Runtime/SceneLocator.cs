// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable ConvertIfStatementToReturnStatement
using System.Collections.Generic;
using System.Linq;
using Ensharp.UnityLocator.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable enable

namespace Ensharp.UnityLocator
{
    /// <summary>
    /// Sceneが切り替わるまで生存させておくServiceLocator管理用クラス
    /// </summary>
    public static class SceneLocator
    {
        // key => scene, value => scene毎のロケーター保持クラス
        private static Dictionary<Scene, ILocatorContainer> _locatorContainers = new();
        
        /// <summary>
        /// serviceをlocatorに登録する
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="locatorService"></param>
        /// <typeparam name="TLocatorService"></typeparam>
        public static void Register<TLocatorService>(Scene scene, TLocatorService? locatorService)
            where TLocatorService : class, ILocatorService
        {
            var locatorContainer = EnsureSceneLocatorExists(scene);
            locatorContainer.Register(locatorService);
        }

        /// <summary>
        /// locatorに登録されているserviceを削除する
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="service"></param>
        /// <typeparam name="TLocatorService"></typeparam>
        public static void Unregister<TLocatorService>(Scene scene, TLocatorService service)
            where TLocatorService : class, ILocatorService
        {
            var locatorContainer = EnsureSceneLocatorExists(scene);
            locatorContainer.Unregister(service);
        }

        /// <summary>
        /// locatorにtypeof(service)が登録されているか確認する
        /// </summary>
        /// <param name="scene"></param>
        /// <typeparam name="TLocatorService"></typeparam>
        /// <returns></returns>
        public static bool IsRegistered<TLocatorService>(Scene scene)
            where TLocatorService : class, ILocatorService
        {
            var locatorContainer = EnsureSceneLocatorExists(scene);
            return locatorContainer.IsRegistered<TLocatorService>();
        }

        /// <summary>
        /// locatorから指定した型のserviceを取得する
        /// </summary>
        /// <param name="scene"></param>
        /// <typeparam name="TLocatorService"></typeparam>
        /// <returns></returns>
        public static TLocatorService? Get<TLocatorService>(Scene scene)
            where TLocatorService : class, ILocatorService
        {
            var locatorContainer = EnsureSceneLocatorExists(scene);
            return locatorContainer.Resolve<TLocatorService>();
        }

        /// <summary>
        /// locatorから指定した型があるかを返し取得する
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="service"></param>
        /// <typeparam name="TLocatorService"></typeparam>
        /// <returns></returns>
        public static bool TryGet<TLocatorService>(Scene scene, out TLocatorService? service)
            where TLocatorService : class, ILocatorService
        {
            var locatorContainer = EnsureSceneLocatorExists(scene);
            return locatorContainer.TryResolve<TLocatorService>(out service);
        }

        /// <summary>
        /// 渡された引数のsceneが登録されていなければ作成を行い追加する
        /// </summary>
        /// <returns>sceneに紐づくLocatorContainer</returns>
        private static ILocatorContainer EnsureSceneLocatorExists(Scene scene)
        {
            if (_locatorContainers.TryGetValue(scene, out var container)) 
                return container;
            
            var createdContainer = new LocatorContainer();
            _locatorContainers.Add(scene, createdContainer);

            return createdContainer;
        }
        
        /// <summary>
        /// 渡された引数のsceneに登録されているLocatorContainerを削除する
        /// </summary>
        /// <returns>sceneに紐づくLocatorContainerが存在していた場合はtrue、存在しなかった場合はfalse</returns>
        private static bool RemoveSceneLocator(Scene scene)
        {
            if (_locatorContainers.TryGetValue(scene, out var container))
            {
                container.DisposeRegisteredServices();
                return _locatorContainers.Remove(scene);
            }
            return false;
        }
        
        /// <summary>
        /// 保持している全てのLocatorContainerを削除する
        /// </summary>
        /// <returns>sceneに紐づくLocatorContainerが存在していた場合はtrue、存在しなかった場合はfalse</returns>
        private static void RemoveAllSceneLocator()
        {
            if (!_locatorContainers.Any()) return;
            
            foreach (var sceneLocator in _locatorContainers)
            {
                sceneLocator.Value.DisposeRegisteredServices();
            }
        }

        /// <summary>
        /// sceneに対応するLocatorContainerを取得する
        /// </summary>
        /// <param name="scene">取得するLocatorContainerに紐づくscene</param>
        /// <returns>sceneに紐づくLocatorContainer</returns>
        private static ILocatorContainer? GetLocatorContainer(Scene scene)
        {
            return _locatorContainers.GetValueOrDefault(scene);
        }


        /// <summary>
        /// ゲーム起動時にLocatorServiceが残らないように初期化を行う
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            UnloadAllSceneLocators();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            Application.quitting += OnApplicationQuit;
            
            return;

            // イベント回りの解放
            void OnApplicationQuit()
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                SceneManager.sceneUnloaded -= OnSceneUnloaded;
                Application.quitting -= OnApplicationQuit;
            }
        }

        private static void UnloadAllSceneLocators()
        {
            RemoveAllSceneLocator();
            
            _locatorContainers = new Dictionary<Scene, ILocatorContainer>();
        }

        /// <summary>
        /// シーンが読み込まれた際、そのsceneに紐づくLocatorContainerを作成する
        /// </summary>
        private static void OnSceneLoaded(Scene loadScene, LoadSceneMode loadSceneMode)
        {
            // 対象のシーンのLocatorContainerを作成する
            EnsureSceneLocatorExists(loadScene);
        }
        
        /// <summary>
        /// シーンが破棄された際、そのsceneに紐づくLocatorContainerを破棄する
        /// </summary>
        private static void OnSceneUnloaded(Scene scene)
        {
            // 対象のシーンのLocatorContainerが存在する場合破棄する
            RemoveSceneLocator(scene);
        }
    }
}
