using Data.Input;

namespace Infrastructure.Services.Input
{
    public interface IPlatformInputProvider
    {
        PlayerInput GetInput();
    }
}