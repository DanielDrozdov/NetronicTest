using UnityEngine;

namespace Core.Characters.Player
{
    public class PlayerPositionProvider : MonoBehaviour, IPlayerPositionProvider
    {
        public Vector3 Position => _trm.position;
        
        private Transform _trm;
        
        private void Awake()
        {
            _trm = transform;
        }
    }
}
