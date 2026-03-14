using CardMatch.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardMatch.SO
{
    [System.Serializable]
    public struct GamePrefabs
    {
        public GameObject cardPrefab;
    }

    [System.Serializable]
    public struct GameSprites
    {
        public Sprite[] cardSprites;
        public Sprite backSprite;
    }

    [System.Serializable]
    public struct CardProperties
    {
        [Min(1)] public uint maxCards;
        [Min(0f)] public float flipDuration;
    }

    [System.Serializable]
    public struct LevelLayoutProperties
    {
        public LayoutData cardLayout;
        public Vector2Int gridSpacing;
    }

    [System.Serializable]
    public struct GameLevels
    {
        public LevelData[] levels;
    }

    [System.Serializable]
    public struct LevelAnimationProperties
    {
        public float levelSetupInitialDelay;
        public float waitBeforeNextFlip;
        public float observeDelay;
    }

    [CreateAssetMenu]
    public class GameData : ScriptableObject
    {
        public GamePrefabs gamePrefabs;
        public GameSprites gameSprites;
        public CardProperties cardProperties;
        public LevelLayoutProperties levelLayoutProperties;
        public GameLevels gameLevels;
        public LevelAnimationProperties levelAnimationProperties;
    }
}