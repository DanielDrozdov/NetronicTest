using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Utility;

namespace Pooling
{
    public class GenericPooler
    {
        #region [Fields]

        private readonly Dictionary<AssetReference, GenericPool> PoolerLibrary = new Dictionary<AssetReference, GenericPool>();
        private readonly Dictionary<int, GenericPool> HashReference = new Dictionary<int, GenericPool>();
        private readonly IObjectsFactory _objectsFactory;

        #endregion

        private readonly Vector3 _spawnPoint = new Vector3(0, 1000, 0);

        public GenericPooler(IObjectsFactory objectsFactory)
        {
            _objectsFactory = objectsFactory;
            SceneManager.sceneUnloaded += _ => ClearDeletedObjectsReferencesInPools();
        }

        #region [Pool instantiation methods]

        public virtual async UniTask<T> InstantiateFromPool<T>(AssetReference objAssetReference) where T : MonoBehaviour, IGenericPoolElement
        {
            T returnedObject = await TakeFromPool<T>(objAssetReference);

            returnedObject.IsCommissioned = true;
            returnedObject.Commission();

            return returnedObject;
        }

        public void ReturnToPool(IGenericPoolElement element)
        {
            HashReference.TryGetValue(element.PoolRef, out GenericPool genericPool);

#if DEBUG
            if (genericPool == null)
            {
                Debug.LogError($"GenericPooler Pool ReUse() call failed on {element.GameObject}. "
                               + "Make sure you're using correct poolHashRef to the correct pool (*.Instance mismatch?)",
                    element.GameObject);
            }
#endif

            element.Decommission();
            genericPool.ReturnToPool(element);
        }

        public async UniTask CreatePrefabsPool(AssetReference assetReference, int prefabsCount, Transform poolTrm = null)
        {
            int remainderToSpawn;

            if (PoolerLibrary.TryGetValue(assetReference, out GenericPool manipulatedPool))
            {
                remainderToSpawn = prefabsCount - manipulatedPool.TotalElementsCount;
            }
            else
            {
                manipulatedPool = CreatePool(assetReference, poolTrm);
                remainderToSpawn = prefabsCount;
            }

            for (int i = 0; i < remainderToSpawn; i++)
            {
                IGenericPoolElement genericPoolElement = await InstantiateNewObjectInPool(manipulatedPool);
                genericPoolElement.GameObject.SetActive(false);
            }
        }

        public virtual void RemoveFromPool(IGenericPoolElement genericObject)
        {
            HashReference.TryGetValue(genericObject.PoolRef, out GenericPool genericPool);

            genericPool?.RemoveFromPool(genericObject);
        }

        private async UniTask<T> TakeFromPool<T>(AssetReference assetReference, Transform poolTrm = null) where T : MonoBehaviour, IGenericPoolElement
        {

            if (!PoolerLibrary.TryGetValue(assetReference, out GenericPool manipulatedPool))
            {
                manipulatedPool = CreatePool(assetReference, poolTrm);
            }

            T returnedObject = await ObtainElement<T>(manipulatedPool);
#if DEBUG
            if (returnedObject == null)
            {
                Debug.LogError("GenericPooler<" + typeof(T) + ">:: InstantiateFromPool() "
                               + "- Type mismatch! Cannot instantiate object by reference: " + assetReference + " even with unity instantiate!");
                return null;
            }
#endif

            return returnedObject;
        }

        #endregion

        private GenericPool CreatePool(AssetReference prefabAssetReference, Transform poolTrm)
        {
            GenericPool pool = new GenericPool(prefabAssetReference, poolTrm);

            PoolerLibrary.Add(prefabAssetReference, pool);
            HashReference.Add(pool.GetHashCode(), pool);
            return pool;
        }

        protected virtual async UniTask<T> ObtainElement<T>(GenericPool pool) where T : MonoBehaviour
        {
            if (pool.HasAvailableElements)
            {
                return pool.PoolFirst<T>();
            }

            await InstantiateNewObjectInPool<T>(pool);

            return pool.PoolFirst<T>();
        }

        protected virtual async UniTask<T> InstantiateNewObjectInPool<T>(GenericPool pool) where T : MonoBehaviour
        {
            IGenericPoolElement element = await InstantiateNewObjectInPool(pool);
            return element as T;
        }

        protected virtual async UniTask<IGenericPoolElement> InstantiateNewObjectInPool(GenericPool pool)
        {
            GameObject pooledObject = await _objectsFactory.Create(pool.PrefabAssetReference, _spawnPoint);
            pooledObject.transform.parent = pool.ParentTransform;

            if (pooledObject.TryGetComponent(out IGenericPoolElement element))
            {
                pool.AddToPool(element);
            }

            return element;
        }

        private void ClearDeletedObjectsReferencesInPools()
        {
            foreach (var genericPool in PoolerLibrary.Values)
            {
                genericPool.ClearDeletedObjectsReferences();
            }
        }
    }
}