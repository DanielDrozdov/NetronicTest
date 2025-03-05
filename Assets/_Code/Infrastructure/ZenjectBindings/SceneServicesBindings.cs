using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
using Zenject;

namespace Infrastructure.ZenjectBindings
{
    public class SceneServicesBindings : MonoInstaller
    {
        public override void InstallBindings()
        {
            InjectSceneDiContainerInObjectsFactory();
            
            InjectDependenciesInLevel();
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