using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using Zenject;
using Object = UnityEngine.Object;

namespace Core
{
    public class ResourceLoader
    {
        private Transform _parentlessPool;

        private DiContainer _container;
        
        [Inject]
        public void Init(DiContainer container)
        {
            _parentlessPool = new GameObject().transform;
            _parentlessPool.name = "[ParentlessPool]";

            _container = container;
            
            Addressables.InitializeAsync();
        }

        private async UniTask<IList<IResourceLocation>> LoadResourceLocationsAsync(object[] keys)
        {
            var locations = await Addressables.LoadResourceLocationsAsync(keys as IEnumerable, Addressables.MergeMode.Union);

            return (locations);
        }

        public bool ReleaseInstance(GameObject instance)
        {
            return Addressables.ReleaseInstance(instance);
        }

        public string GetLocation<T>(string locationSuffix = null)
        {
            var location = typeof(T).GetCustomAttribute<PrefabInfo>().Location;
            if (!string.IsNullOrEmpty(locationSuffix))
            {
                location += locationSuffix;
            }

            return location;
        }

        #region Instantiate

        public async UniTask<TP> Instantiate<TP, TV>(Transform parent, string locationSuffix, params object[] args)
            where TP : SimplePresenter<TP, TV> where TV : SimplePresenterView<TP, TV>
        {
            var view = await Instantiate<TV>(GetLocation<TV>(locationSuffix), parent);

            if (view != null)
            {
                return (TP)view.Instantiate(args);
            }

            return null;
        }

        public async UniTask<T> Instantiate<T>(string key, Transform parent) where T : Component
        {
            var locations = await LoadResourceLocationsAsync(new object[] { key });

            if (locations.Count <= 0)
            {
                throw new NullReferenceException($"locations we can't but we'll let in {key}");
            }

            var handle = Addressables.InstantiateAsync(locations[0], parent ? parent : _parentlessPool);
            
            var prefab = await handle;
            
            var component = prefab.GetComponent<T>();

            _container.Inject(component); 

            return component;
        }

        public async UniTask<GameObject> Instantiate(string key, Transform parent)
        {
            var locations = await LoadResourceLocationsAsync(new object[] { key });

            if (locations.Count <= 0) return null;

            var handle = Addressables.InstantiateAsync(locations[0], parent ? parent : _parentlessPool);
            var prefab = await handle;
            _container.InjectGameObject(prefab);
            return prefab;
        }

        public async UniTask<T> GetResource<T>(string key) where T : Object
        {
            var locations = await LoadResourceLocationsAsync(new object[] { key });

            if (locations.Count <= 0) return null;

            var handle = Addressables.LoadAssetAsync<T>(locations[0]);
            return await handle;
        }

        #endregion
    }
}