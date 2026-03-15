using System;
using System.Collections;
using System.Linq;
using CardMatch.Entities;
using CardMatch.Events;
using CardMatch.SO;
using CardMatch.Utils;
using UnityEngine;

namespace CardMatch.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private RectTransform tableRoot;
        [SerializeField] private UISwitcher uiSwitcher;

        [Header("Data")] [SerializeField] private GameData gameData;
        
        [Header("Events")]
        [SerializeField] private OnLevelLoaded onLevelLoaded;
        [SerializeField] private OnScoreUpdated onScoreUpdated;
        [SerializeField] private OnAllLevelsCompleted onAllLevelsCompleted;
        [SerializeField] private OnContinue onContinue;
        [SerializeField] private OnNewGame onNewGame;
        [SerializeField] private OnExitGame onExitGame;

        private ObjectPool<Card> _cardPool;

        private LevelManager _levelManager;
        private TableManager _tableManager;
        private ScoreManager _scoreManager;

        private Card _previouslySelectedCard;

        private void Awake()
        {
            _cardPool = new ObjectPool<Card>(gameData.gamePrefabs.cardPrefab);
            _cardPool.Generate(gameData.cardProperties.maxCards, card => card.OnClick += OnCardClick);

            _tableManager = new TableManager(gameData, tableRoot, _cardPool);
            _levelManager = new LevelManager(gameData, _tableManager, onLevelLoaded, onAllLevelsCompleted);

            _scoreManager = new ScoreManager(onScoreUpdated);
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
        }

        private void OnDisable()
        {
            onLevelLoaded.Unsubscribe(OnLevelLoaded);
            onAllLevelsCompleted.Unsubscribe(OnAllLevelsCompleted);
            onNewGame.Unsubscribe(OnNewGame);
            onContinue.Unsubscribe(OnContinueGame);
            onExitGame.Unsubscribe(OnExitGame);
        }

        private void OnLevelLoaded(LevelData levelData)
        {
            _scoreManager.OnLevelLoaded(levelData);
        }

        private void OnCardClick(Card card)
        {
            card.IsInteractable = false;
            card.FlipToFront();

            if (_previouslySelectedCard != null)
            {
                if (card.FrontSprite == _previouslySelectedCard.FrontSprite)
                {
                    _scoreManager.OnMatchFound();
                }
                else
                {
                    // wrong match
                    Debug.LogError("Wrong match");
                }

                _previouslySelectedCard = null;
            }
            else
            {
                _previouslySelectedCard = card;
            }

            if (ReachedEndCondition())
                StartCoroutine(ContinueToNextLevel());
        }
        
        private bool ReachedEndCondition()
        {
            return _tableManager.Cards.All(card=>card.IsFront);
        }

        private IEnumerator ContinueToNextLevel()
        {
            yield return new WaitForSeconds(gameData.endConditionProperties.waitBeforeContinue);
            
            yield return _levelManager.LoadNextLevel();
        }

        private void OnAllLevelsCompleted()
        {
            uiSwitcher.Switch(ScreenType.MainMenu);
        }

        private void OnContinueGame()
        {
            
        }

        private void OnNewGame()
        {
            uiSwitcher.Switch(ScreenType.Gameplay);
            StartCoroutine(_levelManager.LoadNextLevel());
        }

        private void OnExitGame()
        {
            Application.Quit();
        }
    }
}