using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Managers.Controller;
using Managers.Interfaces;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace FBXPool
{
    public class FBXPoolItem : MonoBehaviour
    {
        [SerializeField] private ParticleSystem fbxEffect;
        public string AddressableAssetGUID;
        protected FBXPoolController FbxPoolController;

        private CancellationTokenSource _playToken;

        [Inject]
        public void Init(DiContainer container)
        {
            FbxPoolController = container.Resolve<IGameplayManager>().GetController<FBXPoolController>();
            _playToken = new CancellationTokenSource();
        }

        public virtual void Activation()
        {
            transform.DOKill();
            gameObject.SetActive(true);
            fbxEffect.Play();
            
            _playToken = new CancellationTokenSource();
            CheckPlay().Forget();
        }

        public virtual void ReturnToPool()
        {
            _playToken.Cancel();
            fbxEffect.Stop();
            transform.DOKill();

            gameObject.SetActive(false);

            FbxPoolController.BackToPool(this);
        }

        private async UniTask CheckPlay()
        {
            await UniTask.WaitUntil(() => !fbxEffect.IsAlive(true), cancellationToken: _playToken.Token);
            ReturnToPool();
        }

        private void OnDestroy()
        {
            _playToken.Cancel();
        }

#if UNITY_EDITOR
        [ContextMenu("Print My Addressable GUID")]
        private void PrintMyAddressableGuid()
        {
            string path = AssetDatabase.GetAssetPath(gameObject);
            string guid = AssetDatabase.AssetPathToGUID(path);
            AddressableAssetGUID = guid;
        }
#endif
    }
}