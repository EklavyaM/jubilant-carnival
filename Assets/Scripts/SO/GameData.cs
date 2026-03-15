using CardMatch.Data;
using UnityEngine;

namespace CardMatch.SO
{
    [CreateAssetMenu(menuName = "SO/Data/Game Data")]
    public class GameData : ScriptableObject
    {
        public GamePrefabs gamePrefabs;
        public GameSprites gameSprites;
        public CardProperties cardProperties;
        public LevelLayoutProperties levelLayoutProperties;
        public GameLevels gameLevels;
        public LevelAnimationProperties levelAnimationProperties;
        public EndConditionProperties endConditionProperties;
        public AudioProperties audioProperties;
    }
}