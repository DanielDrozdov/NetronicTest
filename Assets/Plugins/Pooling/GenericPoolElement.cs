using UnityEngine;
using Zenject;

namespace Pooling
{
    public class GenericPoolElement : MonoBehaviour, IGenericPoolElement
    {
        public Transform PoolParentTrm { get; set; }
        public GameObject GameObject => gameObject;
        public int PoolRef { get; set; }
        public bool IsCommissioned { get; set; }
        public bool IsDestroyed { get; private set; }
        
        
        private IGenericPoolsReturner _genericPoolsReturner;

        [Inject]
        private void Construct(IGenericPoolsReturner genericPoolsReturner)
        {
            _genericPoolsReturner = genericPoolsReturner;
        }
        
        protected virtual void OnDestroy()
        {
            IsDestroyed = true;
        }

        public virtual void Commission()
        {
            gameObject.SetActive(true);
        }

        public virtual void Decommission()
        {
            gameObject.SetActive(false);
        }

        protected void ReturnObjectToPool()
        {
            _genericPoolsReturner.ReturnObjToPool(this);
        }
    }
}