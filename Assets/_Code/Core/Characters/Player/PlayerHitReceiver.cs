using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Characters.Player
{
    public class PlayerHitReceiver : MonoBehaviour, IPlayerHitReceiver
    {
        [SerializeField, FoldoutGroup("Components")]
        private CharacterHealth _health;
        
        public void Hit(int damage)
        {
            _health.Damage(damage);
        }
    }
}
