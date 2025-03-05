using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility;
using Zenject;

namespace Pooling
{
    public class GenericPoolsProvider : IGenericPoolsProvider
    {
        private GenericPooler _genericPooler;

        [Inject]
        private void Construct(IObjectsFactory objectsFactory)
        {
            _genericPooler = new GenericPooler(objectsFactory);
        }

        public async UniTask CreatePrefabsPool(AssetReference assetReference, Transform poolTrm = null, int prefabsCount = 1)
        {
            await _genericPooler.CreatePrefabsPool(assetReference, prefabsCount, poolTrm);
        }
        
        public async UniTask<T> GetObjFromPool<T>(AssetReference objReference) where T : MonoBehaviour, IGenericPoolElement
        {
            return await _genericPooler.InstantiateFromPool<T>(objReference);
        }

        public void ReturnObjToPool<T>(T genericElement) where T : IGenericPoolElement
        {
            _genericPooler.ReturnToPool(genericElement);
        }
    }
}
