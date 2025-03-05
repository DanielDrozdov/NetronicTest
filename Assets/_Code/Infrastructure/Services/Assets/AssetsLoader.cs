using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Infrastructure.Assets
{
    public class AssetsLoader : IAssetsLoader
    {
        public async UniTask<T> LoadAssetAsync<T>(AssetReference assetReference)
        {
            AsyncOperationHandle<T> operationHandle = Addressables.LoadAssetAsync<T>(assetReference);
            return await ProcessAssetLoad(operationHandle, assetReference);
        }
        
        public async UniTask<GameObject> InstantiateGameObjectAsync(AssetReference assetReference, Vector3 position, Quaternion rotation)
        {
            AsyncOperationHandle<GameObject> operationHandle = Addressables.InstantiateAsync(assetReference, position, rotation);
            return await ProcessAssetLoad(operationHandle, assetReference);
        }

        private async UniTask<T> ProcessAssetLoad<T>(AsyncOperationHandle<T> operationHandle, AssetReference assetReference)
        {
            await operationHandle.Task;

            CheckOnFailedStatus(operationHandle, assetReference);            
            
            return operationHandle.Result;
        }

        private void CheckOnFailedStatus(AsyncOperationHandle operationHandle, AssetReference assetReference)
        {
            if (operationHandle.Status == AsyncOperationStatus.Failed)
            {
                throw new Exception("Asset load failed. Reference: " + assetReference);
            }
        }
    }
}
