using UnityEngine;

namespace Core.Characters.Enemies.Attack
{
    [RequireComponent(typeof(EnemyAttackPossibilityTracker))]
    public abstract class EnemyAttack : MonoBehaviour
    {
        [SerializeField] 
        protected int _damage;
        
        public abstract void Attack(Vector3 playerPos);
    }
}