using Data.Input;
using Infrastructure.Services.Input;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Characters.Player
{
    public class PlayerRotator : MonoBehaviour
    {
        [SerializeField] 
        private float _rotationSpeed;

        [SerializeField] 
        private float _xAxisMinAngle;
        
        [SerializeField] 
        private float _xAxisMaxAngle;
        
        [SerializeField, FoldoutGroup("Components")]
        private Transform _cameraTrm;
        
        [SerializeField, ReadOnly, FoldoutGroup("Components")]
        private PlayerUpdateReceiver _updateReceiver;
        
        private Transform _trm;
        private IApplicationInputProvider _applicationInputProvider;
        private float _xRotation;

        [Inject]
        private void Construct(IApplicationInputProvider applicationInputProvider)
        {
            _applicationInputProvider = applicationInputProvider;
        }
        
        private void Awake()
        {
            _trm = transform;
            _updateReceiver.OnReceivedUpdateCall.Subscribe(_ => Rotate()).AddTo(this);
        }

        private void Rotate()
        {
            PlayerInput playerInput = _applicationInputProvider.GetPlayerInput();
            Vector2 rotationDelta = playerInput.LookVector;
            
            _trm.rotation *= Quaternion.Euler(new  Vector3(0, rotationDelta.x * _rotationSpeed, 0));
            
            float xRotationDelta = rotationDelta.y * _rotationSpeed;
            _xRotation = Mathf.Clamp(_xRotation + xRotationDelta, _xAxisMinAngle, _xAxisMaxAngle); 
            _cameraTrm.localEulerAngles = new Vector3(_xRotation, 0, 0);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_updateReceiver == null) TryGetComponent(out _updateReceiver);
        }
#endif
    }
}