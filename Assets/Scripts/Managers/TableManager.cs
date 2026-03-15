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

        public List<Card> Cards => _cards;
        
        public TableManager(GameData gameData, RectTransform tableRoot, ObjectPool<Card> cardPool)
        {
            _gameData = gameData;
            _tableRoot = tableRoot;
            _cardPool = cardPool;
        }

        private bool GetSpritePairs(LayoutData layoutData, out List<Sprite> spritePairs)
        {
            int uniqueCardsRequired = Mathf.FloorToInt((layoutData.x * layoutData.y) * 0.5f);

            if (uniqueCardsRequired > _gameData.gameSprites.cardSprites.Length)
            {
                spritePairs = null;
                return false;
            }
            
            List<Sprite> allSprites = new List<Sprite>(_gameData.gameSprites.cardSprites);
            spritePairs = new List<Sprite>();

            for (int i = 0; i < uniqueCardsRequired; ++i)
            {
                Sprite randomSprite = allSprites[Random.Range(0, allSprites.Count)];
                
                spritePairs.Add(randomSprite);
                spritePairs.Add(randomSprite); 
                
                allSprites.Remove(randomSprite);
            }
            
            return true;
        }

        public bool TryGenerate(LayoutData layout)
        {
            if ((layout.x * layout.y) % 2 != 0)
            {
                Debug.LogError("Layout does not support pairs. Total elements must be an even number");
                return false;
            }

            if (!GetSpritePairs(layout, out List<Sprite> sprites))
            {
                Debug.LogError("Not enough sprites available to make unique pairs. Try adding more.");
                return false;
            }

            Clear();
            
            float containerWidth = _tableRoot.rect.width;
            float containerHeight = _tableRoot.rect.height;

            float maxWidth = (containerWidth - _gameData.levelLayoutProperties.gridSpacing.x * (layout.x - 1)) / layout.x;
            float maxHeight = (containerHeight - _gameData.levelLayoutProperties.gridSpacing.y * (layout.y - 1)) / layout.y;

            float cardWidth = maxWidth;
            float cardHeight = cardWidth / _gameData.levelLayoutProperties.cardLayout.Aspect;

            if (cardHeight > maxHeight)
            {
                cardHeight = maxHeight;
                cardWidth = cardHeight * _gameData.levelLayoutProperties.cardLayout.Aspect;
            }

            float gridWidth = layout.x * cardWidth + (layout.x - 1) * _gameData.levelLayoutProperties.gridSpacing.x;
            float gridHeight = layout.y * cardHeight + (layout.y - 1) * _gameData.levelLayoutProperties.gridSpacing.y;

            float startX = -gridWidth * 0.5f + cardWidth * 0.5f;
            float startY = gridHeight * 0.5f - cardHeight * 0.5f;
            
            for (int row = 0; row < layout.y; row++)
            {
                for (int col = 0; col < layout.x; col++)
                {
                    if (!_cardPool.TryGet(out Card card))
                    {
                        Debug.LogError("Unable to create enough cards. Try increasing pool size.");
                        Clear();
                        return false;
                    }
                    
                    Sprite randomSprite = sprites[Random.Range(0, sprites.Count)];
                    card.FrontSprite = randomSprite;
                    sprites.Remove(randomSprite);

                    RectTransform cardTransform = card.GetComponent<RectTransform>();
                    cardTransform.SetParent(_tableRoot, false);

                    cardTransform.localScale = Vector3.one;

                    cardTransform.sizeDelta = new Vector2(cardWidth, cardHeight);

                    float x = startX + col * (cardWidth + _gameData.levelLayoutProperties.gridSpacing.x);
                    float y = startY - row * (cardHeight + _gameData.levelLayoutProperties.gridSpacing.y);

                    cardTransform.anchoredPosition = new Vector2(x, y);

                    _cards.Add(card);
                }
            }

            return true;
        }

        public void Clear()
        {
            foreach (Card card in _cards)
            {
                card.Clear();
                _cardPool.Release(card);
            }
            
            _cards.Clear();
        }
    }
}