using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Zenject;

namespace Pooling
{
    public class ElementsPool<T> : MonoBehaviour where T : MonoBehaviour, IGenericPoolElement
    {
        [SerializeField]
        private int _startElementsSize;
        
        [SerializeField]
        private string _poolName;
        
        [SerializeField] 
        private AssetReference _prbAssetRef;

        private IGenericPoolsProvider _genericPoolsProvider;

        public int StartElementsSize => _startElementsSize;
        
        [Inject]
        private void Construct(IGenericPoolsProvider genericPoolsProvider)
        {
            _genericPoolsProvider = genericPoolsProvider;
        }

        private void Awake()
        {
            InitializePool();
        }

        public async UniTask<T> Get()
        {
            return await _genericPoolsProvider.GetObjFromPool<T>(_prbAssetRef);
        }

        public void Return(IGenericPoolElement element)
        {
            _genericPoolsProvider.ReturnObjToPool(element);
        }

        private void InitializePool()
        {
            GameObject parent = new GameObject(_poolName);
            _genericPoolsProvider.CreatePrefabsPool(_prbAssetRef, parent.transform, _startElementsSize);
        }
    }
}