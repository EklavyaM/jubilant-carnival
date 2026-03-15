using System.Linq;
using UnityEngine;

namespace CardMatch.Data
{
    public enum FXType
    {
        Swipe,
        Match,
        Mismatch,
        GameComplete
    }

    [System.Serializable]
    public struct AudioData
    {
        public FXType fxType;
        public AudioClip audioClip;
    }

    [System.Serializable]
    public struct AudioProperties
    {
        [SerializeField] private AudioData[] audioProperties;

        public AudioClip Get(FXType fxType)
        {
            return audioProperties.FirstOrDefault(data => data.fxType == fxType).audioClip;
        }
    }
}