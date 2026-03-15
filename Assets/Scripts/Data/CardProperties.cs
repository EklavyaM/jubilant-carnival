using UnityEngine;

namespace CardMatch.Data
{
    [System.Serializable]
    public struct CardProperties
    {
        [Min(1)] public uint maxCards;
        [Min(0f)] public float flipDuration;
    }
}