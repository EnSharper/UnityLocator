using System.Collections;
using Ensharp.UnityLocator.Core;
using NUnit.Framework;
using UnityEngine.TestTools;

#nullable enable

namespace Ensharp.UnityLocator.Tests
{
    public class GameLocatorTest
    {
        private class TestService : ILocatorService
        {

        }

        [UnitySetUp]
        public void Setup()
        {
            if (GameLocator.TryResolve<TestService>(out var locator))
            {
                GameLocator.Unregister(locator);
            }
        }


        [UnityTest]
        public IEnumerator SceneLocatorTestAsync()
        {
            var locator = new TestService();

            // 登録
            GameLocator.Register(locator);

            // 登録できているか？
            Assert.IsTrue(GameLocator.IsRegistered<TestService>());

            // 取得ができるか？
            Assert.AreEqual(locator, GameLocator.Resolve<TestService>());

            Assert.IsTrue(GameLocator.TryResolve<TestService>(out var locator2));
            Assert.AreEqual(locator, locator2);

            // 削除
            GameLocator.Unregister(locator);

            // 登録削除できているか？
            Assert.IsTrue(GameLocator.Resolve<TestService>() == null);

            Assert.IsFalse(GameLocator.TryResolve<TestService>(out var locator3));
            Assert.IsTrue(locator3 == null);

            yield return null;
        }
    }
}
