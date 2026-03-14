using System.Collections.Generic;
using CardMatch.Data;
using CardMatch.SO;
using UnityEngine;

namespace CardMatch.Managers
{
    public class TableManager : MonoBehaviour
    {
        [Header("Data")] [SerializeField] private GameData gameData;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private RectTransform tableRoot;

        [Header("Editor")] [SerializeField] private LayoutData testLayout;
        [SerializeField] private bool testMode;

        private readonly List<GameObject> _cards = new List<GameObject>();

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (testMode)
                TryGenerate(testLayout);
#endif
        }

        public bool TryGenerate(LayoutData layout)
        {
            if ((layout.x * layout.y) % 2 != 0)
            {
                Debug.LogError("Layout cannot be created");
                return false;
            }

            Clear();

            float containerWidth = tableRoot.rect.width;
            float containerHeight = tableRoot.rect.height;

            float maxWidth = (containerWidth - gameData.GridSpacing.x * (layout.x - 1)) / layout.x;
            float maxHeight = (containerHeight - gameData.GridSpacing.y * (layout.y - 1)) / layout.y;

            float cardWidth = maxWidth;
            float cardHeight = cardWidth / gameData.CardLayout.Aspect;

            if (cardHeight > maxHeight)
            {
                cardHeight = maxHeight;
                cardWidth = cardHeight * gameData.CardLayout.Aspect;
            }

            float gridWidth = layout.x * cardWidth + (layout.x - 1) * gameData.GridSpacing.x;
            float gridHeight = layout.y * cardHeight + (layout.y - 1) * gameData.GridSpacing.y;

            float startX = (containerWidth - gridWidth) / 2f;
            float startY = -(containerHeight - gridHeight) / 2f;

            for (int row = 0; row < layout.y; row++)
            {
                for (int col = 0; col < layout.x; col++)
                {
                    GameObject card = Instantiate(cardPrefab, tableRoot);
                    RectTransform cardTransform = card.GetComponent<RectTransform>();

                    cardTransform.sizeDelta = new Vector2(cardWidth, cardHeight);

                    float x = startX + col * (cardWidth + gameData.GridSpacing.x);
                    float y = startY - row * (cardHeight + gameData.GridSpacing.y);

                    cardTransform.anchoredPosition = new Vector2(x, y);

                    _cards.Add(card);
                }
            }

            return true;
        }

        public void Clear()
        {
            foreach (GameObject card in _cards)
                Destroy(card);
            _cards.Clear();
        }
    }
}