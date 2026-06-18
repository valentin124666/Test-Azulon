using System;

using Core;
using Managers.Controller;
using Managers.Interfaces;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelPresenter : SimplePresenter<LevelPresenter, LevelPresenterView>, ILevel
    {
        private PlayerController _playerController;
        
     

        public LevelPresenter(LevelPresenterView view, ResourceLoader resourceLoader) : base(view, resourceLoader)
        {
        }

        [Inject]
        public void Init(DiContainer container)
        {
            _playerController = container.Resolve<IGameplayManager>().GetController<PlayerController>();
        }

        public float GetProgressLevel()
        {
            return 0;
        }
    }
}