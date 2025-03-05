using System;
using Core.Characters.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core.Characters.Enemies.Attack
{
    public class EnemyCloseAttack : EnemyAttack
    {
        [SerializeField] 
        private Vector3 _attackBoxSize;
        
        [SerializeField]
        private LayerMask _playerLayerMask;
        
        [SerializeField, FoldoutGroup("Components")] 
        private Transform _attackBoxTrm;
        
        private IPlayerHitReceiver _playerHitReceiver;

        [Inject]
        private void Construct(IPlayerHitReceiver playerHitReceiver)
        {
            _playerHitReceiver = playerHitReceiver;
        }
        
        public override void Attack(Vector3 playerPos)
        {
            int results = 
                Physics.OverlapBoxNonAlloc(_attackBoxTrm.position, _attackBoxSize / 2, null, Quaternion.identity, _playerLayerMask);
            
            if (results == 1)
            {
                _playerHitReceiver.Hit(_damage);
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color color = Color.red;
            color.a = 0.2f;
            Gizmos.color = color;
            
            Gizmos.DrawCube(_attackBoxTrm.position, _attackBoxSize);
        }
        #endif
    }
}