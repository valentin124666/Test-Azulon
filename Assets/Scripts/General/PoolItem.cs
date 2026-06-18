using DG.Tweening;
using Managers.Controller;
using Settings;
using UnityEngine;

namespace General
{
    public class PoolItem 
    {
        protected PoolController _poolController;

        public Enumerators.NamePrefabAddressable NamePrefab { get; }

        public GameObject gameObject { get; }
        public Transform transform => gameObject.transform;

        public PoolItem(Enumerators.NamePrefabAddressable name, GameObject gameObject,PoolController poolController)
        {
            gameObject.name += "[PoolItem]";
            
            NamePrefab = name;
            _poolController = poolController;
            gameObject.SetActive(false);
            this.gameObject = gameObject;
        }
        
        public virtual void Activation( params object[] args)
        {
            transform.DOKill();
            gameObject.SetActive(true);
        }

        public virtual void ReturnToPool()
        {
            transform.DOKill();

            gameObject.SetActive(false);
        
            _poolController.BackToPool(this);
        }
    }
}