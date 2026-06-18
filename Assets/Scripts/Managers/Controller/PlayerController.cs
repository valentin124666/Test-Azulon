using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Cysharp.Threading.Tasks;
using Data;
using Items;
using Items.Effects;
using Managers.Interfaces;
using Modules;
using Modules.Interfaces;
using Modules.Player;
using Modules.Player.Commands;
using Modules.Player.Modules;
using Player.Commands;
using UnityEngine;
using Zenject;

namespace Managers.Controller
{
    public class PlayerController : IController
    {
        public event Action<BasePlayerGlobalEvent> PlayerGlobalEvent;
        public event Action UpdatedBackpack;

        private event Action<IPlayerCommand> OnPlayerCommandReceived;

        public bool IsInit { get; private set; }

        private IPlayerPresenter _currentPlayer;

        private ItemEquipHandler _itemEquipHandler;
        private ItemConsumeHandler _itemConsumeHandler;

        private Transform _poolPlayer;
        private ResourceLoader _resourceLoader;

        private PlayerBackpack _playerBackpack;

        private BasePlayerModule[] _playerModule;

        private CombatModule combatModule;
        private HealthModule healthModule;
        private StatsModule statsModule;

        private int coinCollectedInLevel;

        #region Options

        public int MaxHealth => healthModule.MaxHealth;
        public int CurrentHealth => healthModule.CurrentHealth;

        public float XpBoost => statsModule.XpBoost;
        public float CurrentSpeed => statsModule.CurrentSpeed;

        public int Attack => combatModule.Attack;
        public int Defense => combatModule.Defense;

        #endregion

        public void Init(DiContainer container)
        {
            _resourceLoader = container.Resolve<ResourceLoader>();

            _playerModule = new BasePlayerModule[]
            {
                combatModule = new CombatModule(),
                healthModule = new HealthModule(),
                statsModule = new StatsModule()
            };

            _itemEquipHandler = new ItemEquipHandler(this);
            _itemConsumeHandler = new ItemConsumeHandler(this);

            foreach (BasePlayerModule playerModule in _playerModule)
            {
                OnPlayerCommandReceived += playerModule.Apply;
            }

            _playerBackpack = new PlayerBackpack(this);

            _poolPlayer = new GameObject("[PoolPlayer]").transform;
            IsInit = true;
        }

        private void HandlePlayerPresenterEvent(BasePlayerPresenterEvent playerPresenterEvent)
        {
            switch (playerPresenterEvent)
            {
            }
        }


        private async UniTask WaitPresenterCreated(Action callback)
        {
            await UniTask.WaitUntil(() => _currentPlayer != null);
            callback.Invoke();
        }

        public void StartGameplay()
        {
            if (_currentPlayer == null)
            {
                WaitPresenterCreated(() => _currentPlayer.HandleEventMassage(new PEMStartAction())).Forget();
            }
            else
            {
                _currentPlayer.HandleEventMassage(new PEMStartAction());
            }
        }

        public void CollectItem(ItemData itemData)
        {
            _playerBackpack.AddItem(itemData);

            UpdatedBackpack?.Invoke();
        }

        public void UseItem(string IdSlot)
        {
            var item = _playerBackpack.GetItem(IdSlot);

            if (item.InteractionType == ItemInteractionType.Consume)
            {
                _itemConsumeHandler.UseItem(item);
            }
            else
            {
                _itemEquipHandler.Equip(item);
            }
        }

        public void AddEffects(ItemEffectBase[] itemEffects)
        {
            foreach (var item in itemEffects)
            {
                OnPlayerCommandReceived?.Invoke(item.Apply());
            }
        }

        public void RemoveEffects(ItemEffectBase[] itemEffects)
        {
            foreach (var item in itemEffects)
            {
                OnPlayerCommandReceived?.Invoke(item.Remove());
            }
        }

        public async UniTask CreatePlayerPresentersAsync(PlayerPrefab playerPrefab)
        {
            coinCollectedInLevel = 0;

            if (_currentPlayer != null)
            {
                _currentPlayer.PlayerPresenterEvent -= HandlePlayerPresenterEvent;
            }


            _currentPlayer = null;


            var presenterType = Type.GetType(playerPrefab.presenterType, true, true);

            if (presenterType == typeof(PlayerPresenter))
            {
                _currentPlayer =
                    await _resourceLoader.Instantiate<PlayerPresenter, PlayerPresenterView>(_poolPlayer.transform,
                        playerPrefab.locationSuffix);
            }

            _currentPlayer.PlayerPresenterEvent += HandlePlayerPresenterEvent;

            var equipmentSlot = (PPDREquipmentSlot)_currentPlayer.RetrievePlayerInfo(new PPDRGetEquipmentSlot());

            _itemEquipHandler.SetEquipSlot(equipmentSlot.equipmentSlot);
        }

        public BasePlayerDataRequests RetrievePlayerInfo(BasePlayerDataCommands playerDataCommands)
        {
            switch (playerDataCommands)
            {
                case PDCGetCameraAnchor:
                    if (_currentPlayer != null)
                    {
                        return new PDRCameraAnchor(
                            (PPDRCameraAnchor)_currentPlayer.RetrievePlayerInfo(new PPDCGetCameraAnchor()));
                    }

                    return new BasePlayerDataRequests();
                
                case PDCGetStatsInterface:
                    if (_currentPlayer != null)
                    {
                        return new PDRStatsInterface(_playerModule.OfType<IPlayerModuleStats>().ToArray());
                    }    
                    return new BasePlayerDataRequests();
                
                case PDCGetPlayerBackpack:
                    return new PDRPlayerBackpack(_playerBackpack);
                default:
                    return new BasePlayerDataRequests();
            }
        }

        public void Reset()
        {
            _currentPlayer.PlayerPresenterEvent -= HandlePlayerPresenterEvent;
            _currentPlayer.OnDestroy();
            _currentPlayer = null;
        }

        public void Restart()
        {
            coinCollectedInLevel = 0;
        }
    }
}