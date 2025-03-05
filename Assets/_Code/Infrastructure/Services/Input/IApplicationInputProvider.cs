using Data.Input;

namespace Infrastructure.Services.Input
{
    public interface IApplicationInputProvider
    {
        PlayerInput GetPlayerInput();
        bool IsInputBlocked();
    }
}