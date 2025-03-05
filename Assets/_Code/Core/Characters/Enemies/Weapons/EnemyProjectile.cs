using Core.Characters.Player;
using Pooling;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Characters.Enemies.Weapons
{
    [RequireComponent(typeof(EnemyProjectileTrigger))]
    public class EnemyProjectile : GenericPoolElement
    {
        [SerializeField] 
        private float _speed;
        
        [SerializeField, ReadOnly, FoldoutGroup("Components")]
        private Rigidbody _rb;
        
        [SerializeField, ReadOnly, FoldoutGroup("Components")]
        private EnemyProjectileTrigger _trigger;

        private IPlayerHitReceiver _playerHitReceiver;
        private Vector3 _moveDirection;
        private int _damage;

        [Inject]
        private void Construct(IPlayerHitReceiver playerHitReceiver)
        {
            _playerHitReceiver = playerHitReceiver;
        }
        
        private void Awake()
        {
            _trigger.OnEnterPlayerCollider.Subscribe(_ => OnEnterPlayerCollider()).AddTo(this);
        }

        private void FixedUpdate()
        {
            _rb.velocity = _moveDirection * _speed;
        }

        public override void Decommission()
        {
            base.Decommission();
            _rb.velocity = Vector3.zero;
        }

        public void Throw(Vector3 direction, int damage)
        {
            _moveDirection = direction;
            _damage = damage;
        }

        private void OnEnterPlayerCollider()
        {
            _playerHitReceiver.Hit(_damage);
            ReturnObjectToPool();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_rb == null) TryGetComponent(out _rb);
            
            if (_trigger == null) TryGetComponent(out _trigger);
        }
#endif
    }
}
