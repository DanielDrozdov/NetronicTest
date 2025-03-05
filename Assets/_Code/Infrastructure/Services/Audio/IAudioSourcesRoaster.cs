using UnityEngine;

namespace Infrastructure.Audio
{
    public interface IAudioSourcesRoaster
    {
        AudioSource GetFreeAudioSource();
        void ReturnAudioSource(AudioSource audioSource);
    }
}