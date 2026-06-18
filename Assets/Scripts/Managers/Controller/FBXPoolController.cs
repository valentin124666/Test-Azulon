using System;
using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using Data;
using FBXPool;
using Managers.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Object = UnityEngine.Object;

namespace Managers.Controller
{
    public class FBXPoolController : IController
    {
        private Dictionary<string,Queue<FBXPoolItem>> _fbxPoolPrefabs;
        
        private HashSet<FBXPoolItem> _allFBXPoolPrefabs;
        private ResourceLoader _resourceLoader;
        private IGameDataManager _gameDataManager;

        private FBXPoolData _fbxPoolData;

        private Transform _poolFBX;
        private DiContainer _container;
        public event Action PoolNpcCreated;

        public bool IsInit { get; private set; }
        
        public bool PoolCreateCompleted{ get; private set; }

        public void Init(DiContainer container)
        {
            _container = container;

            _poolFBX = new GameObject("[PoolFBX]").transform;
            _resourceLoader = container.Resolve<ResourceLoader>();
            _gameDataManager = container.Resolve<IGameDataManager>();

            _fbxPoolPrefabs = new Dictionary<string, Queue<FBXPoolItem>>();
            _allFBXPoolPrefabs = new HashSet<FBXPoolItem>();
            _gameDataManager.DataDownload += CreatePool;
            IsInit = true;
        }
        
        private void CreatePool()
        {
            _fbxPoolData = _gameDataManager.GetDataScriptable<GameData>().FBXPoolData;
            CreatePoolAsync().Forget();
        }

        private async UniTask CreatePoolAsync()
        {
            var tasks = new UniTask[_fbxPoolData.FBXPoolPrefabs.Count];
            
            for (var i = 0; i < _fbxPoolData.FBXPoolPrefabs.Count; i++)
            {
                var FBXPrefab = _fbxPoolData.FBXPoolPrefabs[i];
                var viewType = Type.GetType(FBXPrefab.viewType, true, true);

                if (viewType == typeof(FBXPoolItem))
                {
                    tasks[i] = CreatePoolItemAsync<FBXPoolItem>(FBXPrefab.numberOfExamples, "FBX/" + FBXPrefab.locationSuffix);
                }
 
               
            }

            await UniTask.WhenAll(tasks);

            PoolCreateCompleted = true;
            PoolNpcCreated?.Invoke();
        }
        private async UniTask CreatePoolItemAsync<T>(int numberOfPassenger, string locationSuffix) where T : FBXPoolItem
        {
            var prefab = await _resourceLoader.GetResource<GameObject>(locationSuffix);

            var prefabPool = prefab.GetComponent<T>();
            
            if (!_fbxPoolPrefabs.ContainsKey(prefabPool.AddressableAssetGUID))
            {
                _fbxPoolPrefabs.Add(prefabPool.AddressableAssetGUID, new Queue<FBXPoolItem>());
            }

            for (int i = 0; i < numberOfPassenger; i++)
            {
                var poolItem = Object.Instantiate(prefabPool, _poolFBX);
                
                _container.Inject(poolItem);
                poolItem.gameObject.SetActive(false);
                _fbxPoolPrefabs[prefabPool.AddressableAssetGUID].Enqueue(poolItem);
                _allFBXPoolPrefabs.Add(poolItem);
            }

            Addressables.Release(prefab);
        }

        public FBXPoolItem GetFBX( string assetGUID) 
        {
            if (!_fbxPoolPrefabs.ContainsKey(assetGUID))
            {
                Debug.Log($"No pool for assetGUID {assetGUID}");
                return null;
            }

            return _fbxPoolPrefabs[assetGUID].Dequeue();
        }
        
        public void BackToPool(FBXPoolItem item)
        {
            var type = item.AddressableAssetGUID;

            if (_fbxPoolPrefabs.ContainsKey(type))
            {
                _fbxPoolPrefabs[type].Enqueue(item);
            }
        }
        
        
    }
}
