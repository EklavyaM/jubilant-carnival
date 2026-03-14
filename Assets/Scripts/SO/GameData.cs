using CardMatch.Data;
using UnityEngine;

namespace CardMatch.SO
{
    [CreateAssetMenu]
    public class GameData : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject cardPrefab;
        
        [Header("Sprites")]
        [SerializeField] private Sprite[] cardSprites;
        [SerializeField] private Sprite backSprite;
        
        [Header("Properties")] [SerializeField, Min(0f)]
        private float flipDuration;
        
        [Header("Layout")]
        [SerializeField] private LayoutData cardLayout;
        [SerializeField] private Vector2Int gridSpacing;
        
        [Header("Levels")]
        [SerializeField] private LevelData[] levels;
        
        public GameObject CardPrefab => cardPrefab;
        public Sprite[] CardSprites => cardSprites;
        public LayoutData CardLayout => cardLayout;
        public Vector2Int GridSpacing => gridSpacing;
        public LevelData[] Levels => levels;
        public Sprite BackSprite => backSprite;
        public float FlipDuration => flipDuration;
    }
}