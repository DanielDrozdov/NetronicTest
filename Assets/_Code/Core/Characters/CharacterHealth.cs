using UnityEngine;

namespace Core.Characters
{
    public class CharacterHealth : MonoBehaviour
    {
        [SerializeField] 
        private int _health;

        private bool _isDead;
        
        public void Damage(int value)
        {
            if (_isDead) return; 
            
            Debug.Log("Hit");
            _health -= value;

            if (_health <= 0)
            {
                _isDead = true;
                Debug.Log("Die");
            }
        }
    }
}
