using System;
using Core.Characters.Enemies.Weapons;
using Cysharp.Threading.Tasks;
using Pooling;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Core.Characters.Enemies.Attack
{
    public class EnemyProjectileAttack : EnemyAttack
    {
        [SerializeField] 
        private AssetReference _projectileAssetRef;

        private Transform _trm;
        private IGenericPoolsProvider _genericPoolsProvider;

        [Inject]
        private void Construct(IGenericPoolsProvider genericPoolsProvider)
        {
            _genericPoolsProvider = genericPoolsProvider;
        }

        private void Awake()
        {
            _trm = transform;
        }

        public override void Attack(Vector3 playerPos)
        {
            ThrowProjectile(playerPos);
        }

        private async UniTaskVoid ThrowProjectile(Vector3 playerPos)
        {
            EnemyProjectile enemyProjectile = await _genericPoolsProvider.GetObjFromPool<EnemyProjectile>(_projectileAssetRef);
            enemyProjectile.transform.position = _trm.position;
            enemyProjectile.Throw((playerPos -  _trm.position).normalized, _damage);
        }
    }
}