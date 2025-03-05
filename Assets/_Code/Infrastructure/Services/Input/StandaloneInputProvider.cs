using Data.Input;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class StandaloneInputProvider : IPlatformInputProvider
    {
        private const string _horizontalMoveKey = "Horizontal";
        private const string _verticalMoveKey = "Vertical";
        private const string _horizontalLookKey = "Mouse X";
        private const string _verticalLookKey = "Mouse Y";
        
        public PlayerInput GetInput()
        {
            return new PlayerInput()
            {
                MovementVector = new Vector2(UnityEngine.Input.GetAxis(_horizontalMoveKey), UnityEngine.Input.GetAxis(_verticalMoveKey)),
                LookVector = new Vector2(UnityEngine.Input.GetAxis(_horizontalLookKey), UnityEngine.Input.GetAxis(_verticalLookKey))
            };
        }
    }
}
