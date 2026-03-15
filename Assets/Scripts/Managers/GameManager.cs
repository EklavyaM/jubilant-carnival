using System;
using System.Collections;
using System.Linq;
using CardMatch.Data;
using CardMatch.Entities;
using CardMatch.SO.Events;
using CardMatch.SO;
using CardMatch.SO.Funcs;
using CardMatch.Utils;
using UnityEngine;

namespace CardMatch.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private RectTransform tableRoot;

        [SerializeField] private UISwitcher uiSwitcher;
        [SerializeField] private AudioSource audioSource;

        [Header("Data")] [SerializeField] private GameData gameData;

        [Header("Events")] [SerializeField] private OnLevelLoaded onLevelLoaded;
        [SerializeField] private OnScoreUpdated onScoreUpdated;
        [SerializeField] private OnAllLevelsCompleted onAllLevelsCompleted;
        [SerializeField] private OnContinue onContinue;
        [SerializeField] private OnNewGame onNewGame;
        [SerializeField] private OnExitGame onExitGame;
        [SerializeField] private OnReturnToMenu onReturnToMenu;

        [Header("Funcs")] [SerializeField] private CanContinue canContinue;

        private ObjectPool<Card> _cardPool;

        private AudioManager _audioManager;
        private LevelManager _levelManager;
        private TableManager _tableManager;
        private ScoreManager _scoreManager;
        private SaveDataManager _saveDataManager;

        private Card _previouslySelectedCard;

        private Coroutine _levelLoadRoutine;
        private Coroutine _continueRoutine;

        private void Awake()
        {
            _cardPool = new ObjectPool<Card>(gameData.gamePrefabs.cardPrefab);
            _cardPool.Generate(gameData.cardProperties.maxCards, card => card.OnClick += OnCardClick);

            _tableManager = new TableManager(gameData, tableRoot, _cardPool);
            _levelManager = new LevelManager(gameData, _tableManager, onLevelLoaded, onAllLevelsCompleted);

            _scoreManager = new ScoreManager(onScoreUpdated);
            _saveDataManager = new SaveDataManager();
            _audioManager = new AudioManager(gameData, audioSource);
        }

        private void Start()
        {
            uiSwitcher.Switch(ScreenType.MainMenu);
        }

        private void OnEnable()
        {
            onLevelLoaded.Subscribe(OnLevelLoaded);
            onAllLevelsCompleted.Subscribe(OnAllLevelsCompleted);
            onNewGame.Subscribe(OnNewGame);
            onContinue.Subscribe(OnContinueGame);
            onExitGame.Subscribe(OnExitGame);
            onReturnToMenu.Subscribe(OnReturnToMenu);
            canContinue.Subscribe(CanContinueGame);
        }

        private void OnDisable()
        {
            onLevelLoaded.Unsubscribe(OnLevelLoaded);
            onAllLevelsCompleted.Unsubscribe(OnAllLevelsCompleted);
            onNewGame.Unsubscribe(OnNewGame);
            onContinue.Unsubscribe(OnContinueGame);
            onExitGame.Unsubscribe(OnExitGame);
            onReturnToMenu.Unsubscribe(OnReturnToMenu);
            canContinue.Unsubscribe(CanContinueGame);
        }

        private void OnLevelLoaded(LevelData levelData)
        {
            _scoreManager.OnLevelLoaded(levelData);
            _saveDataManager.OnLevelLoaded(levelData);
        }

        private void OnCardClick(Card card)
        {
            card.IsInteractable = false;
            card.FlipToFront();
            
            _audioManager.Play(FXType.Swipe);

            if (_previouslySelectedCard != null)
            {
                if (card.FrontSprite == _previouslySelectedCard.FrontSprite)
                {
                    _scoreManager.OnMatchFound();
                    _audioManager.Play(FXType.Match);
                }
                else
                {
                    _audioManager.Play(FXType.Mismatch);
                }

                _previouslySelectedCard = null;
            }
            else
            {
                _previouslySelectedCard = card;
            }

            if (ReachedEndCondition())
            {
                _audioManager.Play(FXType.GameComplete);
                _continueRoutine = StartCoroutine(ContinueToNextLevel());
            }
        }

        private bool ReachedEndCondition()
        {
            return _tableManager.Cards.All(card => card.IsFront);
        }

        private IEnumerator ContinueToNextLevel()
        {
            yield return new WaitForSeconds(gameData.endConditionProperties.waitBeforeContinue);

            yield return _levelManager.LoadNextLevel();
        }

        private void OnAllLevelsCompleted()
        {
            ResetGameplay();
            uiSwitcher.Switch(ScreenType.MainMenu);
        }

        private void OnContinueGame()
        {
            if (!CanContinueGame())
                return;
            
            uiSwitcher.Switch(ScreenType.Gameplay);

            int index = Array.FindIndex(gameData.gameLevels.levels,
                data => string.Equals(_saveDataManager.SaveData.levelId, data.levelId));
            index = Mathf.Clamp(index, 0, gameData.gameLevels.levels.Length - 1);

            _levelLoadRoutine = StartCoroutine(_levelManager.LoadNextLevel(index));
        }

        private bool CanContinueGame()
        {
            int index = Array.FindIndex(gameData.gameLevels.levels,
                data => string.Equals(_saveDataManager.SaveData.levelId, data.levelId));
            return index >= 0 && index < gameData.gameLevels.levels.Length;
        }

        private void OnNewGame()
        {
            uiSwitcher.Switch(ScreenType.Gameplay);
            _levelLoadRoutine = StartCoroutine(_levelManager.LoadNextLevel());
        }

        private void OnReturnToMenu()
        {
            ResetGameplay();
            uiSwitcher.Switch(ScreenType.MainMenu);
        }

        private void OnExitGame()
        {
            Application.Quit();
        }

        private void ResetGameplay()
        {
            if (_levelLoadRoutine != null)
                StopCoroutine(_levelLoadRoutine);
            if (_continueRoutine != null)
                StopCoroutine(_continueRoutine);

            _levelManager.Clear();
        }
    }
}