namespace Ensharp.UnityLocator.Core
{
    public interface ILocatorContainer
    {
        /// <summary>
        /// サービスを登録する
        /// </summary>
        /// <param name="locatorService">登録するサービス</param>
        /// <typeparam name="TLocatorService">サービスの型情報</typeparam>
        public void Register<TLocatorService>(TLocatorService locatorService)
            where TLocatorService : class, ILocatorService;

        /// <summary>
        /// 登録されているサービスを解除する
        /// </summary>
        /// <param name="service">解除するサービス</param>
        /// <typeparam name="TLocatorService">解除するサービスの型情報</typeparam>
        public void Unregister<TLocatorService>(TLocatorService service)
            where TLocatorService : class, ILocatorService;

        /// <summary>
        /// サービスが登録されているか確認する
        /// </summary>
        /// <typeparam name="TLocatorService">確認するサービスの型情報</typeparam>
        /// <returns></returns>
        public bool IsRegistered<TLocatorService>()
            where TLocatorService : class, ILocatorService;
        
        /// <summary>
        /// 登録されているサービスを取得する
        /// </summary>
        /// <typeparam name="TLocatorService">取得するサービスの型情報</typeparam>
        /// <returns></returns>
        public TLocatorService Resolve<TLocatorService>()
            where TLocatorService : class, ILocatorService;

        /// <summary>
        /// 登録されているサービスを取得する。無ければ戻り値がfalseとなる
        /// </summary>
        /// <param name="service">対象のサービスが存在した際、そのサービスが渡される</param>
        /// <typeparam name="TLocatorService">取得するサービスの型情報</typeparam>
        /// <returns></returns>
        public bool TryResolve<TLocatorService>(out TLocatorService service)
            where TLocatorService : class, ILocatorService;

        /// <summary>
        /// 登録されているサービスを破棄する
        /// </summary>
        public void DisposeRegisteredServices();
    }
}
