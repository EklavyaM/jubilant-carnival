using CardMatch.Data;
using UnityEngine;

namespace CardMatch.Data
{
    [System.Serializable]
    public struct LevelLayoutProperties
    {
        public LayoutData cardLayout;
        public Vector2Int gridSpacing;
    }
}