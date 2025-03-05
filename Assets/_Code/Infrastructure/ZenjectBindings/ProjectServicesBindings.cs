using Data.Save;
using Infrastructure.Assets;
using Infrastructure.Audio;
using Infrastructure.Factory;
using Infrastructure.Services.Input;
using Pooling;
using UnityEngine;
using Utility;
using Zenject;

namespace Infrastructure.ZenjectBindings
{
    public class ProjectServicesBindings : MonoInstaller
    {
        [SerializeField]
        private AudioPlayer _audioPlayer;
        
        public override void InstallBindings()
        {
            // Main Bindings
            BindGameDataSaver();
            BindAssetsLoader();
            BindObjectsFactory();
            BindObjectsPool();
            BindApplicationInputProvider();
            
            // Secondary Bindings
            BindAudioPlayer();
        }

        #region Main Bindings
        
        private void BindGameDataSaver()
        {
            Container
                .Bind<IGameDataAccess>()
                .To<GameDataSaver>()
                .AsSingle()
                .NonLazy();
        }

        private void BindAssetsLoader()
        {
            Container
                .Bind<IAssetsLoader>()
                .To<AssetsLoader>()
                .AsSingle()
                .NonLazy();
        }

        private void BindObjectsFactory()
        {
            Container
                .Bind<IObjectsFactory>()
                .To<ObjectsFactory>()
                .AsSingle()
                .NonLazy();
        }

        private void BindObjectsPool()
        {
            Container
                .BindInterfacesTo<GenericPoolsProvider>()
                .AsSingle()
                .NonLazy();
        }

        private void BindApplicationInputProvider()
        {
            Container
                .BindInterfacesTo<ApplicationInputProvider>()
                .AsSingle()
                .NonLazy();
        }
        
        #endregion
        
        private void BindAudioPlayer()
        {
            Container
                .Bind<IAudioPlayer>()
                .To<AudioPlayer>()
                .FromInstance(_audioPlayer)
                .AsSingle()
                .NonLazy();
        }
    }
}
