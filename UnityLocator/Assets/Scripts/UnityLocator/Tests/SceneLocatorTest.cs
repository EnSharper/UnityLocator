using System.Collections;
using Ensharp.UnityLocator.Core;
using Ensharp.UnityLocator.LocatorExtends;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

#nullable enable

namespace Ensharp.UnityLocator.Tests
{
    public class SceneLocatorTest
    {
        private class TestService : ILocatorService
        {

        }

        private GameObject? _locatorObject;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _locatorObject = new GameObject("LocatorEntryPointObject");
        }

        [UnitySetUp]
        public void Setup()
        {
            if (_locatorObject == null)
            {
                _locatorObject = new GameObject("LocatorEntryPointObject");
            }

            if (_locatorObject.TryResolveSceneLocator<TestService>(out var locator))
            {
                _locatorObject.UnregisterSceneLocator(locator);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Object.DestroyImmediate(_locatorObject);
        }

        [UnityTest]
        public IEnumerator SceneLocatorTestAsync()
        {
            var locator = new TestService();

            // 登録
            _locatorObject.RegisterSceneLocator(locator);

            // 登録できているか？
            Assert.IsTrue(_locatorObject.IsRegisteredSceneLocator<TestService>());

            // 取得ができるか？
            Assert.AreEqual(locator, _locatorObject.ResolveSceneLocator<TestService>());

            Assert.IsTrue(_locatorObject.TryResolveSceneLocator<TestService>(out var locator2));
            Assert.AreEqual(locator, locator2);

            // 削除
            _locatorObject.UnregisterSceneLocator(locator);

            // 登録削除できているか？
            Assert.IsTrue(_locatorObject.ResolveSceneLocator<TestService>() == null);

            Assert.IsFalse(_locatorObject.TryResolveSceneLocator<TestService>(out var locator3));
            Assert.IsTrue(locator3 == null);

            yield return null;
        }
    }
}
