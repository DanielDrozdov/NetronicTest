using UnityEngine;

namespace Core.Characters.Player
{
    public interface IPlayerPositionProvider
    {
        Vector3 Position { get; }
    }
}