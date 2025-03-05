using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Pooling
{
    public interface IGenericPoolsProvider : IGenericPoolsReturner
    {
        UniTask CreatePrefabsPool(AssetReference assetReference, Transform poolTrm = null, int prefabsCount = 1);
        UniTask<T> GetObjFromPool<T>(AssetReference objReference) where T : MonoBehaviour, IGenericPoolElement;
    }
}