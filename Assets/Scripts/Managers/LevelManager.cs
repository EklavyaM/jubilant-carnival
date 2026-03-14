using System.Collections;
using CardMatch.Entities;
using CardMatch.SO;
using CardMatch.Utils;
using UnityEngine;

namespace CardMatch.Managers
{
    public class LevelManager
    {
        private readonly GameData _gameData;
        private readonly ObjectPool<Card> _cardPool;
        private readonly TableManager _tableManager;

        public int LevelIndex { private set; get; } = -1;

        public LevelManager(GameData gameData, RectTransform tableRoot)
        {
            _gameData = gameData;
            _cardPool = new ObjectPool<Card>(gameData.gamePrefabs.cardPrefab);
            _cardPool.Generate(gameData.cardProperties.maxCards);
            
            _tableManager = new TableManager(gameData, tableRoot, _cardPool);
        }

        private IEnumerator LevelSetupAnimation()
        {
            yield return new WaitForSeconds(_gameData.levelAnimationProperties.levelSetupInitialDelay);

            foreach (Card card in _tableManager.Cards)
                card.IsInteractable = false;
            
            foreach (Card card in _tableManager.Cards)
            {
                card.FlipToFront();
                yield return new WaitForSeconds(_gameData.levelAnimationProperties.waitBeforeNextFlip);
            }

            yield return new WaitForSeconds(_gameData.levelAnimationProperties.observeDelay);

            foreach (Card card in _tableManager.Cards)
            {
                card.FlipToBack();
                yield return new WaitForSeconds(_gameData.levelAnimationProperties.waitBeforeNextFlip);
            }
            
            foreach (Card card in _tableManager.Cards)
                card.IsInteractable = true;
        }
        
        public IEnumerator Load()
        {
            ++LevelIndex;
            _tableManager.TryGenerate(_gameData.gameLevels.levels[LevelIndex].layoutData);

            yield return LevelSetupAnimation();
        }
    }
}