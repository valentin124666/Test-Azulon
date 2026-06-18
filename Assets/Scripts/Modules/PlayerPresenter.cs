using System;
using System.Threading;
using Core;
using Items;
using Managers;
using Managers.Controller;
using Modules.Interfaces;
using Modules.Player;
using Modules.Player.Commands;
using Player.Commands;
using Services;
using Tools;
using UnityEngine;
using Zenject;

namespace Modules
{
    public class PlayerPresenter : SimplePresenter<PlayerPresenter, PlayerPresenterView>, IPlayerPresenter
    {
        public event Action<BasePlayerPresenterEvent> PlayerPresenterEvent;

        private PlayerController _playerController;
        
        private float _startPositionInLevel;

        private bool _isUpdateActionView;

        private CancellationTokenSource _ctsLifTime;

        private UpdateService _updateService;


        public PlayerPresenter(PlayerPresenterView view, ResourceLoader resourceLoader) : base(view, resourceLoader)
        {
            view.PlayerEventMassage += HandleEventMassage;
            _ctsLifTime = new CancellationTokenSource();
        }

        [Inject]
        public void Init(DiContainer container)
        {
            _playerController =  container.Resolve<GameplayManager>().GetController<PlayerController>();
            _updateService = container.Resolve<UpdateService>();
            _updateService.UpdateEvent += Update;
            _updateService.FixedUpdateEvent += FixedUpdate;
        }


        public void ResetPos()
        {
            View.ResetModules();
        }

        public BasePlayerPresenterDataRequests RetrievePlayerInfo(BasePlayerPresenterDataCommands playerDataCommands)
        {
            switch (playerDataCommands)
            {
                case PPDCGetCameraAnchor ppdcGetCameraAnchor:
                    return new PPDRCameraAnchor(View.FollowCamera, View.LookAtCamera);   
                case PPDRGetEquipmentSlot ppdcGetEquipmentSlot :
                    return new PPDREquipmentSlot(View.GetModule<EquipmentSlotView>());
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerDataCommands));
            }
        }


        
        public void HandleEventMassage(BasePlayerEventMassage eventMassage)
        {
            switch (eventMassage)
            {
                case PEMStartAction start:
                    _isUpdateActionView = true;

                    break;          
                case PEMAddItem addItem:
                    _playerController.CollectItem(addItem.ItemData);

                    break;
            }
        }


        private void Update()
        {
            if (!_isUpdateActionView)
            {
                return;
            }

            View.UpdateCustom();
        }

        private void FixedUpdate()
        {
            if (!_isUpdateActionView)
            {
                return;
            }

            View.FixedUpdateCustom();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _updateService.UpdateEvent -= Update;
            _updateService.FixedUpdateEvent -= FixedUpdate;
        }
    }
}