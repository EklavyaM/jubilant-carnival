using System.Collections;
using CardMatch.Entities;
using CardMatch.Events;
using CardMatch.SO;
using CardMatch.Utils;
using UnityEngine;

namespace CardMatch.Managers
{
    public class LevelManager
    {
        private readonly OnLevelLoaded _onLevelLoaded;
        private readonly GameData _gameData;
        private readonly TableManager _tableManager;

        public int LevelIndex { private set; get; } = -1;

        public LevelManager(GameData gameData, TableManager tableManager, OnLevelLoaded onLevelLoaded)
        {
            _gameData = gameData;
            _tableManager = tableManager;
            _onLevelLoaded = onLevelLoaded;
        }

        private IEnumerator LevelSetupAnimation()
        {
            foreach (Card card in _tableManager.Cards)
                card.IsInteractable = false;
            
            yield return new WaitForSeconds(_gameData.levelAnimationProperties.levelSetupInitialDelay);

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
            _onLevelLoaded.Raise(_gameData.gameLevels.levels[LevelIndex]);

            yield return LevelSetupAnimation();
        }
    }
}