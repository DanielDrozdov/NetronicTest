using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utility
{
    public interface IObjectsFactory
    {
        UniTask<GameObject> Create(AssetReference assetReference);
        UniTask<GameObject> Create(AssetReference assetReference, Vector3 pos);
        UniTask<GameObject> Create(AssetReference assetReference, Vector3 pos, Quaternion rotation);
    }
}