using System.Collections.Generic;
using CardMatch.Data;
using CardMatch.Entities;
using CardMatch.SO;
using CardMatch.Utils;
using UnityEngine;

namespace CardMatch.Managers
{
    public class TableManager
    {
        private readonly GameData _gameData;
        private readonly RectTransform _tableRoot;
        private readonly ObjectPool<Card> _cardPool;

        private readonly List<Card> _cards = new List<Card>();

        public TableManager(GameData gameData, RectTransform tableRoot, ObjectPool<Card> cardPool)
        {
            _gameData = gameData;
            _tableRoot = tableRoot;
            _cardPool = cardPool;
        }

        public bool TryGenerate(LayoutData layout)
        {
            if ((layout.x * layout.y) % 2 != 0)
            {
                Debug.LogError("Layout cannot be created");
                return false;
            }

            Clear();

            float containerWidth = _tableRoot.rect.width;
            float containerHeight = _tableRoot.rect.height;

            float maxWidth = (containerWidth - _gameData.GridSpacing.x * (layout.x - 1)) / layout.x;
            float maxHeight = (containerHeight - _gameData.GridSpacing.y * (layout.y - 1)) / layout.y;

            float cardWidth = maxWidth;
            float cardHeight = cardWidth / _gameData.CardLayout.Aspect;

            if (cardHeight > maxHeight)
            {
                cardHeight = maxHeight;
                cardWidth = cardHeight * _gameData.CardLayout.Aspect;
            }

            float gridWidth = layout.x * cardWidth + (layout.x - 1) * _gameData.GridSpacing.x;
            float gridHeight = layout.y * cardHeight + (layout.y - 1) * _gameData.GridSpacing.y;

            float startX = (containerWidth - gridWidth) / 2f;
            float startY = -(containerHeight - gridHeight) / 2f;

            for (int row = 0; row < layout.y; row++)
            {
                for (int col = 0; col < layout.x; col++)
                {
                    if (!_cardPool.TryGet(out Card card))
                    {
                        Debug.LogError("Unable to create enough cards");
                        Clear();
                        return false;
                    }
                    
                    RectTransform cardTransform = card.GetComponent<RectTransform>();
                    cardTransform.SetParent(_tableRoot);
                    
                    cardTransform.sizeDelta = new Vector2(cardWidth, cardHeight);

                    float x = startX + col * (cardWidth + _gameData.GridSpacing.x);
                    float y = startY - row * (cardHeight + _gameData.GridSpacing.y);

                    cardTransform.anchoredPosition = new Vector2(x, y);

                    _cards.Add(card);
                }
            }

            return true;
        }

        public void Clear()
        {
            foreach (Card card in _cards)
                _cardPool.Release(card);
            _cards.Clear();
        }
    }
}