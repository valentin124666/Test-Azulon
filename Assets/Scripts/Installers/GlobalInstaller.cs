using Core;
using Managers;
using Managers.Controller;
using Managers.Interfaces;
using Services;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private UpdateService updateService;

        public override void InstallBindings()
        {
            Debug.Log("GlobalBindings");

            Container.Bind<IGameDataManager>().To<GameDataManager>().AsSingle().NonLazy();
            Container.Bind<IUIManager>().To<UIManager>().AsSingle().NonLazy();
            Container.Bind<ISceneLoaderManager>().To<SceneLoaderManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameplayManager>().AsSingle().NonLazy();
            
            Container.Bind<JoystickService>().To<JoystickService>().AsSingle().NonLazy();
            Container.Bind<ResourceLoader>().To<ResourceLoader>().AsSingle().NonLazy();
            Container.Bind<UpdateService>().FromInstance(updateService).AsSingle().NonLazy();
        }
    }
}
