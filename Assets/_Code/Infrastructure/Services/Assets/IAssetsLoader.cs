using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Assets
{
    public interface IAssetsLoader
    {
        UniTask<T> LoadAssetAsync<T>(AssetReference assetReference);
        UniTask<GameObject> InstantiateGameObjectAsync(AssetReference assetReference, Vector3 position, Quaternion rotation);
    }
}