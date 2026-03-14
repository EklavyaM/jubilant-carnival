using CardMatch.Data;
using UnityEngine;

namespace CardMatch.SO
{
    [CreateAssetMenu]
    public class GameData : ScriptableObject
    {
        [SerializeField] private Sprite[] cardSprites;
        [SerializeField] private LayoutData cardLayout;
        [SerializeField] private Vector2Int gridSpacing;
        
        public Sprite[] CardSprites => cardSprites;
        public LayoutData CardLayout => cardLayout;
        public Vector2Int GridSpacing => gridSpacing;
    }
}