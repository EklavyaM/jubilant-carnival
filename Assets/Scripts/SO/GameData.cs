using UnityEngine;

namespace CardMatch.SO
{
    [CreateAssetMenu]
    public class GameData : ScriptableObject
    {
        [SerializeField] private Sprite[] cardSprites;
        [SerializeField] private Vector2 cardAspect;
        
        public Sprite[] CardSprites => cardSprites;
        public Vector2 CardAspect => cardAspect;
    }
}