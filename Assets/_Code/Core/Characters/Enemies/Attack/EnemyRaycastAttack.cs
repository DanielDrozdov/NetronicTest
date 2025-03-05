using System;
using Core.Characters.Player;
using UnityEngine;
using Zenject;

namespace Core.Characters.Enemies.Attack
{
    public class EnemyRaycastAttack : EnemyAttack
    {
        [SerializeField]
        private LayerMask _playerLayerMask;

        private Transform _trm;
        private IPlayerHitReceiver _playerHitReceiver;

        [Inject]
        private void Construct(IPlayerHitReceiver playerHitReceiver)
        {
            _playerHitReceiver = playerHitReceiver;
        }

        private void Awake()
        {
            _trm = transform;
        }

        public override void Attack(Vector3 playerPos)
        {
            Vector3 directionToPlayer = playerPos - _trm.position;
            int results = Physics.RaycastNonAlloc(_trm.position, directionToPlayer, null, 100, _playerLayerMask);
            
            Debug.DrawLine(_trm.position, directionToPlayer, Color.red, 1f); // only for view
            
            if (results == 1)
            {
                _playerHitReceiver.Hit(_damage);
            }
        }
    }
}