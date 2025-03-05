using Pooling;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Characters.Enemies
{
    public class Enemy : GenericPoolElement
    {
        [SerializeField, ReadOnly,  FoldoutGroup("Components")] 
        private EnemyAttackPossibilityTracker _attackPossibilityTracker;

        [SerializeField, ReadOnly, FoldoutGroup("Components")]
        private EnemyMover _mover;
        
        [SerializeField, ReadOnly, FoldoutGroup("Components")]
        private NavMeshAgent _agent;

        public void Place(Vector3 position)
        {
            _agent.Warp(position);
        }
        
        public override void Commission()
        {
            base.Commission();
            _mover.Activate();
            _attackPossibilityTracker.Activate();
        }

        public override void Decommission()
        {
            base.Decommission();
            _mover.Deactivate();
            _attackPossibilityTracker.Deactivate();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_mover == null) TryGetComponent(out _mover);
            
            if (_agent == null) TryGetComponent(out _agent);
            
            if (_attackPossibilityTracker == null) _attackPossibilityTracker = GetComponentInChildren<EnemyAttackPossibilityTracker>();
        }
#endif
    }
}
