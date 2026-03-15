using CardMatch.Data;
using CardMatch.SO;
using UnityEngine;

namespace CardMatch.Managers
{
    public class AudioManager
    {
        private readonly AudioSource _audioSource;
        private readonly GameData _gameData;

        public AudioManager(GameData gameData, AudioSource audioSource)
        {
            _gameData = gameData;
            _audioSource = audioSource;
        }

        public void Play(FXType fxType)
        {
            AudioClip clip = _gameData.audioProperties.Get(fxType);
            _audioSource.PlayOneShot(clip);
        }
    }
}