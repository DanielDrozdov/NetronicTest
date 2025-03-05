using Core.Audio;
using UnityEngine;

namespace Infrastructure.Audio
{
    public interface IAudioPlayer
    {
        void PlayAudio(ItemAudioPlayer audioItem, AudioClip audioClip, Vector3 position, float volume = 1);
        void CancelItemAllAudio(ItemAudioPlayer audioItem);
    }
}