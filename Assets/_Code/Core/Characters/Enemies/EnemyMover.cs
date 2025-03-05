using Core.Characters.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UpdateSys;
using Zenject;

namespace Core.Characters.Enemies
{
    public class EnemyMover : MonoBehaviour, IUpdatable
    {
        [SerializeField] 
        private float _timeToUpdateDestination;
        
        [SerializeField, ReadOnly, FoldoutGroup("Components")]
        private NavMeshAgent _agent;

        private IPlayerPositionProvider _playerPositionProvider;
        private float _timeToUpdateDestinationBalance;

        [Inject]
        private void Construct(IPlayerPositionProvider playerPositionProvider)
        {
            _playerPositionProvider = playerPositionProvider;
        }

        public void OnSystemUpdate(float deltaTime)
        {
            UpdateDestinationTimer(deltaTime);
        }

        private void UpdateDestinationTimer(float deltaTime)
        {
            _timeToUpdateDestinationBalance -= deltaTime;

            if (_timeToUpdateDestinationBalance <= 0)
            {
                UpdateTargetPos();
            }
        }

        public void Activate()
        {
            this.StartUpdate();
        }

        public void Deactivate()
        {
            this.StopUpdate();
        }

        private void UpdateTargetPos()
        {
            _agent.SetDestination(_playerPositionProvider.Position);
            _timeToUpdateDestinationBalance = _timeToUpdateDestination;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_agent == null) TryGetComponent(out _agent);
        }
#endif
    }
}
