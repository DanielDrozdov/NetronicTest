using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Audio
{
    public class AudioSourcesRoaster :  IAudioSourcesRoaster
    {
        private List<AudioSource> _freeAudioSources;
        private const int _audioSourcesCount = 50;
        private const float _startSoundsVolume = 1f;
        
        public AudioSourcesRoaster(GameObject audioSourcePrb, Transform selfTransform)
        {
            InitializeSoundSourcesPool(audioSourcePrb, selfTransform);
        }

        public AudioSource GetFreeAudioSource()
        {
            AudioSource audioSource = _freeAudioSources[0];
            audioSource.gameObject.SetActive(true);
            _freeAudioSources.RemoveAt(0);
            return audioSource;
        }

        public void ReturnAudioSource(AudioSource audioSource)
        {
            audioSource.Stop();
            audioSource.volume = _startSoundsVolume;
            audioSource.gameObject.SetActive(false);
            _freeAudioSources.Add(audioSource);
        }

        private void InitializeSoundSourcesPool(GameObject audioSourcePrb, Transform selfTransform)
        {
            GameObject parent = new GameObject("AudioSources");
            _freeAudioSources = new List<AudioSource>();
            parent.transform.parent = selfTransform;

            for (int i = 0; i < _audioSourcesCount; i++)
            {
                AudioSource audioSource = Object.Instantiate(audioSourcePrb, parent.transform).GetComponent<AudioSource>();
                ReturnAudioSource(audioSource);
            }
        }
    }
}
