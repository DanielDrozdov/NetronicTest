using Data.Input;
using Infrastructure.Services.Input;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Characters.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] 
        private float _speed;
        
        [SerializeField, ReadOnly, FoldoutGroup("Components")]
        private PlayerUpdateReceiver _updateReceiver;
        
        [SerializeField, ReadOnly, FoldoutGroup("Components")]
        private CharacterController _characterController;
        
        private Transform _trm;
        private IApplicationInputProvider _applicationInputProvider;

        [Inject]
        private void Construct(IApplicationInputProvider applicationInputProvider)
        {
            _applicationInputProvider = applicationInputProvider;
        }

        private void Awake()
        {
            _trm = transform;
            _updateReceiver.OnReceivedUpdateCall.Subscribe(_ => Move()).AddTo(this);
        }

        private void Move()
        {
            PlayerInput playerInput = _applicationInputProvider.GetPlayerInput();
            Vector3 forwardVector = playerInput.MovementVector.y * _trm.forward;
            Vector3 rightVector = playerInput.MovementVector.x * _trm.right;
            Vector3 movementVector = (forwardVector + rightVector).normalized;
            _characterController.Move(Time.deltaTime * _speed * movementVector);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_updateReceiver == null) TryGetComponent(out _updateReceiver);
            
            if (_characterController == null) TryGetComponent(out _characterController);
        }
#endif
    }
}
