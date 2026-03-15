using System.Collections;
using CardMatch.Entities;
using CardMatch.SO.Events;
using CardMatch.SO;
using CardMatch.Utils;
using UnityEngine;

namespace CardMatch.Managers
{
    public class LevelManager
    {
        private readonly OnLevelLoaded _onLevelLoaded;
        private readonly OnAllLevelsCompleted _onAllLevelsCompleted;
        private readonly GameData _gameData;
        private readonly TableManager _tableManager;

        public int LevelIndex { private set; get; } = -1;

        public LevelManager(GameData gameData, TableManager tableManager, OnLevelLoaded onLevelLoaded,
            OnAllLevelsCompleted onAllLevelsCompleted)
        {
            _gameData = gameData;
            _tableManager = tableManager;
            _onLevelLoaded = onLevelLoaded;
            _onAllLevelsCompleted = onAllLevelsCompleted;
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

        private IEnumerator UnloadLevel()
        {
            bool anyCardFlipRequired = false;

            foreach (Card card in _tableManager.Cards)
            {
                if (!card.IsFront) continue;

                anyCardFlipRequired = true;

                card.FlipToBack();
                yield return new WaitForSeconds(_gameData.levelAnimationProperties.waitBeforeNextFlip);
            }

            if (anyCardFlipRequired)
                yield return new WaitForSeconds(_gameData.levelAnimationProperties.waitAfterUnload);
        }

        public IEnumerator LoadNextLevel()
        {
            yield return LoadNextLevel(LevelIndex + 1);
        }

        public IEnumerator LoadNextLevel(int index)
        {
            yield return UnloadLevel();

            Clear();

            if (index < 0 || index >= _gameData.gameLevels.levels.Length)
            {
                _onAllLevelsCompleted.Raise();
                yield break;
            }

            LevelIndex = index;

            _tableManager.TryGenerate(_gameData.gameLevels.levels[LevelIndex].layoutData);
            _onLevelLoaded.Raise(_gameData.gameLevels.levels[LevelIndex]);

            yield return LevelSetupAnimation();
        }

        public void Clear()
        {
            LevelIndex = -1;
            _tableManager.Clear();
        }
    }
}