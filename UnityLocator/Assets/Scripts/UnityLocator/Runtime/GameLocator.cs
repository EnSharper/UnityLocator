using Ensharp.UnityLocator.Core;

namespace Ensharp.UnityLocator
{
    /// <summary>
    /// Game起動中常に生存させておくServiceLocator管理用クラス
    /// </summary>
    public static class GameLocator
    {
        private static readonly ILocatorContainer LocatorContainer = new LocatorContainer();

        public static void Register<TLocatorService>(TLocatorService locatorService)
            where TLocatorService : class, ILocatorService
        {
            LocatorContainer.Register(locatorService);
        }

        public static void Unregister<TLocatorService>(TLocatorService service)
            where TLocatorService : class, ILocatorService
        {
            LocatorContainer.Unregister(service);
        }

        public static bool IsRegistered<TLocatorService>()
            where TLocatorService : class, ILocatorService
        {
            return LocatorContainer.IsRegistered<TLocatorService>();
        }

        public static TLocatorService Resolve<TLocatorService>()
            where TLocatorService : class, ILocatorService
        {
            return LocatorContainer.Resolve<TLocatorService>();
        }

        public static bool TryResolve<TLocatorService>(out TLocatorService service)
            where TLocatorService : class, ILocatorService
        {
            return LocatorContainer.TryResolve<TLocatorService>(out service);
        }
    }
}
