using UnityEngine;

namespace Core.Characters
{
    public class CharacterHealth : MonoBehaviour
    {
        [SerializeField] 
        private int _health;

        public void Damage(int value)
        {
            _health -= value;

            if (_health <= 0)
            {
                Debug.Log("Die");
            }
        }
    }
}
