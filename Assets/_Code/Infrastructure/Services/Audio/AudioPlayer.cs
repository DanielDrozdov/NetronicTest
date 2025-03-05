using System.Collections.Generic;
using Core.Audio;
using DG.Tweening;
using UnityEngine;

namespace Infrastructure.Audio
{
    public class AudioPlayer : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] 
        private AudioSource _soundPlayerPrb;
        
        private readonly Dictionary<ItemAudioPlayer, List<AudioSource>> _audioPlayersDict = new Dictionary<ItemAudioPlayer, List<AudioSource>>();
        private IAudioSourcesRoaster _audioSourcesRoaster;
        
        private void Awake()
        {
            _audioSourcesRoaster = new AudioSourcesRoaster(_soundPlayerPrb.gameObject, transform);
        }

        public void PlayAudio(ItemAudioPlayer audioItem, AudioClip audioClip, Vector3 position, float volume)
        {
            AudioSource audioSource = PrepareAudioSourceForPlaying(audioItem, audioClip, position, volume);
            
            DOVirtual.DelayedCall(audioSource.clip.length, () =>
            {
                ReturnAudioSourceToPool(audioItem, audioSource);
            });
        }

        public void CancelItemAllAudio(ItemAudioPlayer audioItem)
        {
            if (_audioPlayersDict.TryGetValue(audioItem, out List<AudioSource> audioSources))
            {
                for (int i = 0; i < audioSources.Count;)
                {
                    ReturnAudioSourceToPool(audioItem, audioSources[i]);
                }
            }
        }

        private AudioSource PrepareAudioSourceForPlaying(ItemAudioPlayer audioItem, AudioClip audioClip, Vector3 position, float volume = 1)
        {
            AudioSource audioSource = _audioSourcesRoaster.GetFreeAudioSource();
            AddSourceToSoundItem(audioItem, audioSource);

            audioSource.transform.position = position;
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.Play();

            return audioSource;
        }

        private void ReturnAudioSourceToPool(ItemAudioPlayer audioItem, AudioSource audioSource)
        {
            _audioPlayersDict[audioItem].Remove(audioSource);
            _audioSourcesRoaster.ReturnAudioSource(audioSource);
        }
        
        private void AddSourceToSoundItem(ItemAudioPlayer audioItem, AudioSource audioSource)
        {
            if (!_audioPlayersDict.ContainsKey(audioItem))
            {
                _audioPlayersDict[audioItem] = new List<AudioSource>();
            }
            
            _audioPlayersDict[audioItem].Add(audioSource);
        }
    }
}
