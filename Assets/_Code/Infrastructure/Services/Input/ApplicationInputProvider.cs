using Data.Input;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class ApplicationInputProvider : IApplicationInputProvider, IApplicationInputBlocker
    {
        private readonly IPlatformInputProvider _platformInputProvider;
        private bool _isInputBlocked;
        
        public ApplicationInputProvider()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                _platformInputProvider = new StandaloneInputProvider();
            }
        }

        public bool IsInputBlocked() => _isInputBlocked;

        public PlayerInput GetPlayerInput()
        {
            if (_isInputBlocked)
            {
                return new PlayerInput();
            }
            
            return _platformInputProvider.GetInput();
        }

        public void BlockUserInput()
        {
            _isInputBlocked = true;
        }

        public void UnblockUserInput()
        {
            _isInputBlocked = false;
        }
    }
}
