using System;
using UniRx;
using UnityEngine;

namespace Core.Characters.Enemies.Weapons
{
    public class EnemyProjectileTrigger : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _playerLayerMask;

        private int _playerLayerMaskNumber;

        private readonly ReactiveCommand _onEnterPlayerCollider = new ReactiveCommand();
        
        public IObservable<Unit> OnEnterPlayerCollider => _onEnterPlayerCollider;

        private void Awake()
        {
            _playerLayerMaskNumber = (int)Mathf.Log(_playerLayerMask.value, 2);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_playerLayerMaskNumber != other.gameObject.layer) return;

            _onEnterPlayerCollider.Execute();
        }
    }
}