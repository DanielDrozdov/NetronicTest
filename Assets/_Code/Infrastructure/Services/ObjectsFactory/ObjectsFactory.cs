using Cysharp.Threading.Tasks;
using Infrastructure.Assets;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility;
using Zenject;

namespace Infrastructure.Factory
{
    public class ObjectsFactory : IObjectsFactory
    {
        private DiContainer _diContainer;
        private readonly IAssetsLoader _assetsLoader;

        public ObjectsFactory(IAssetsLoader assetsLoader)
        {
            _assetsLoader = assetsLoader;
        }
        
        [Inject]
        private void Construct(DiContainer container)
        {
            _diContainer = container;
        }

        public async UniTask<GameObject> Create(AssetReference assetReference)
        {
            return await Create(assetReference, VectorConstants.Zero, Quaternion.identity);
        }

        public async UniTask<GameObject> Create(AssetReference assetReference, Vector3 pos)
        {
            return await Create(assetReference, pos, Quaternion.identity);
        }

        public async UniTask<GameObject> Create(AssetReference assetReference, Vector3 pos, Quaternion rotation)
        {
            GameObject instantiatedObj = await _assetsLoader.InstantiateGameObjectAsync(assetReference, pos, rotation);
            instantiatedObj.AddComponent<SelfResourceReleaser>();
            _diContainer.InjectGameObject(instantiatedObj);
            return instantiatedObj;
        }
    }
}
