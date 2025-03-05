using Infrastructure.Audio;
using UnityEngine;
using Zenject;

namespace Core.Audio
{
    public class ItemAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _audioClip;
        
        private IAudioPlayer _audioPlayer;

        [Inject]
        private void Construct(IAudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

        public void PlayAudio(float volume = 1f)
        {
            _audioPlayer.PlayAudio(this, _audioClip, transform.position, volume);
        }

        public void CancelAudioPlaying()
        {
            _audioPlayer.CancelItemAllAudio(this);
        }
    }
}
