using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Interfaces;
using Items;
using Level;
using Modules.Interfaces;
using Player.Commands;
using UnityEngine;

namespace Modules
{
    [PrefabInfo("Players/")]
    public class PlayerPresenterView : SimplePresenterView<PlayerPresenter, PlayerPresenterView>
    {
        [SerializeField] private Transform followCamera;
        [SerializeField] private Transform lookAtCamera;
        [SerializeField] private Transform modelTransform;

        public event Action<BasePlayerEventMassage> PlayerEventMassage;

        private HashSet<IPlayerModule> _playerModules;
        private HashSet<IUpdatePlayerView> _updatePlayerViews;
        private HashSet<IFixedUpdatePlayerView> _fixedUpdatePlayerViews;

        public Transform FollowCamera => followCamera;
        public Transform LookAtCamera => lookAtCamera;

        public Transform ModelTransform => modelTransform;

        public override void Init()
        {
            _playerModules = GetComponents<IPlayerModule>().ToHashSet();
            foreach (var module in _playerModules)
            {
                module.Init(Container, this);
            }

            _updatePlayerViews = _playerModules.OfType<IUpdatePlayerView>().ToHashSet();
            _fixedUpdatePlayerViews = _playerModules.OfType<IFixedUpdatePlayerView>().ToHashSet();
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<WinGame>(out var winGame))
            {
                winGame.ResetFBX();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<WorldItem>(out var worldItem))
            {
                PlayerEventMassage?.Invoke(new PEMAddItem(worldItem.GetItemData()));
            }
        }

        public void ResetModules()
        {
            foreach (var module in _playerModules)
            {
                module.Reset();
            }
        }

        public void UpdateCustom()
        {
            foreach (var update in _updatePlayerViews)
            {
                update.UpdateCustom();
            }
        }

        public void FixedUpdateCustom()
        {
            foreach (var update in _fixedUpdatePlayerViews)
            {
                update.FixedUpdateCustom();
            }
        }

        public T GetModule<T>() where T : IPlayerModule
        {
            return (T)_playerModules.First(controller => controller is T);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            ResetModules();
        }
    }
}