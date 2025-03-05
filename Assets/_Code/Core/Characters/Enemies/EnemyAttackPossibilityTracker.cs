using Core.Characters.Enemies.Attack;
using Core.Characters.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UpdateSys;
using Zenject;

namespace Core.Characters.Enemies
{
    public class EnemyAttackPossibilityTracker : MonoBehaviour, IUpdatable
    {
        [SerializeField, Unit(Units.Meter)] 
        private float _attackDistance;

        [SerializeField, Unit(Units.Second)] 
        private float _attackCooldown;

        [SerializeField, ReadOnly, FoldoutGroup("Components")]
        private EnemyAttack _selfAttack;

        private Transform _trm;
        private IPlayerPositionProvider _playerPositionProvider;
        private float _attackCooldownBalance;
        private float _attackDistanceSqr;

        [Inject]
        private void Construct(IPlayerPositionProvider playerPositionProvider)
        {
            _playerPositionProvider = playerPositionProvider;
        }

        private void Awake()
        {
            _trm = transform;
            _attackDistanceSqr = _attackDistance * _attackDistance;
            _attackCooldownBalance = _attackCooldown;
        }

        public void OnSystemUpdate(float deltaTime)
        {
            _attackCooldownBalance -= deltaTime;
            
            if (_attackCooldownBalance <= 0 && IsPlayerInAttackDistance())
            {
                _selfAttack.Attack(_playerPositionProvider.Position);
                _attackCooldownBalance = _attackCooldown;
            }
        }

        public void Activate()
        {
            this.StartUpdate();
        }

        public void Deactivate()
        {
            this.StopUpdate();
        }

        private bool IsPlayerInAttackDistance()
        {
            Vector3 playerPos = _playerPositionProvider.Position;
            return (playerPos - _trm.position).sqrMagnitude <= _attackDistanceSqr;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_selfAttack == null) TryGetComponent(out _selfAttack);
        }
#endif
    }
}