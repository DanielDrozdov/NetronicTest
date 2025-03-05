using Core.Characters.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using Zenject;

namespace Infrastructure.ZenjectBindings
{
    public class SceneServicesBindings : MonoInstaller
    {
        [SerializeField]
        private PlayerPositionProvider _playerPositionProvider;
        
        [SerializeField]
        private PlayerHitReceiver _playerHitReceiver;
        
        public override void InstallBindings()
        {
            BindPlayerPositionProvider();
            BindPlayerHitReceiver();
            
            InjectSceneDiContainerInObjectsFactory();
            InjectDependenciesInLevel();
        }

        private void BindPlayerPositionProvider()
        {
            Container
                .Bind<IPlayerPositionProvider>()
                .To<PlayerPositionProvider>()
                .FromInstance(_playerPositionProvider)
                .AsSingle()
                .NonLazy();
        }

        private void BindPlayerHitReceiver()
        {
            Container
                .Bind<IPlayerHitReceiver>()
                .To<PlayerHitReceiver>()
                .FromInstance(_playerHitReceiver)
                .AsSingle()
                .NonLazy();
        }

        private void InjectSceneDiContainerInObjectsFactory()
        {
            IObjectsFactory objectsFactory = Container.Resolve<IObjectsFactory>();
            Container.Inject(objectsFactory);
        }
        
        private void InjectDependenciesInLevel()
        {
            GameObject[] sceneRootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (GameObject obj in sceneRootObjects)
            {
                if (obj == transform.parent.gameObject) return;
                
                Container.InjectGameObject(obj);
            }
        }
    }
}