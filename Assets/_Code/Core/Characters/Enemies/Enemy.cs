using Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Characters.Enemies
{
    public class Enemy : GenericPoolElement
    {
        [SerializeField, ReadOnly,  FoldoutGroup("Components")] 
        private EnemyAttackPossibilityTracker _attackPossibilityTracker;
        
        public override void Commission()
        {
            base.Commission();
            _attackPossibilityTracker.Activate();
        }

        public override void Decommission()
        {
            base.Decommission();
            _attackPossibilityTracker.Deactivate();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_attackPossibilityTracker == null) _attackPossibilityTracker = GetComponentInChildren<EnemyAttackPossibilityTracker>();
        }
#endif
    }
}
