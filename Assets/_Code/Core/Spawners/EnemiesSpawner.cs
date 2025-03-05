using Core.Characters.Enemies;
using Cysharp.Threading.Tasks;
using Pooling;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Random = UnityEngine.Random;

namespace Core.Spawners
{
    public class EnemiesSpawner : MonoBehaviour
    {
        [SerializeField]
        private int _maxEnemies;
        
        [SerializeField, Unit(Units.Second)]
        private float _spawnCooldown;
        
        [SerializeField, FoldoutGroup("Components")]
        private Transform[] _spawnPoints;
        
        [SerializeField, FoldoutGroup("Components"), Space]
        private AssetReference[] _enemiesReferences;

        private IGenericPoolsProvider _poolsProvider;
        private float _remainingTimeToSpawnNextEnemy;

        [Inject]
        private void Construct(IGenericPoolsProvider poolsProvider)
        {
            _poolsProvider = poolsProvider;
        }
        
        private void Awake()
        {
            _remainingTimeToSpawnNextEnemy = _spawnCooldown;
        }

        private void Update()   
        {
            _remainingTimeToSpawnNextEnemy -= Time.deltaTime;

            if (_remainingTimeToSpawnNextEnemy <= 0)
            {
                SpawnEnemy();
            }
        }

        private async UniTaskVoid SpawnEnemy()
        {
            _remainingTimeToSpawnNextEnemy = _spawnCooldown;
            Vector3 randomSpawnPoint = GetRandomSpawnPoint();
            AssetReference enemyRef = GetRandomEnemyRef();
            Enemy enemy = await _poolsProvider.GetObjFromPool<Enemy>(enemyRef);
            enemy.Place(randomSpawnPoint);
        }

        private AssetReference GetRandomEnemyRef()
        {
            int randomIndex = Random.Range(0, _enemiesReferences.Length);
            return _enemiesReferences[randomIndex];
        }

        private Vector3 GetRandomSpawnPoint()
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length);
            return _spawnPoints[randomIndex].position;
        }
    }
}
