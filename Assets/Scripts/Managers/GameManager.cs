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

        [Header("Data")] [SerializeField] private GameData gameData;
        
        [Header("Events")]
        [SerializeField] private OnLevelLoaded onLevelLoaded;
        [SerializeField] private OnScoreUpdated onScoreUpdated;

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
            _levelManager = new LevelManager(gameData, _tableManager, onLevelLoaded);

            _scoreManager = new ScoreManager(onScoreUpdated);
        }

        private void OnEnable()
        {
            onLevelLoaded.Subscribe(OnLevelLoaded);
        }

        private void OnDisable()
        {
            onLevelLoaded.Unsubscribe(OnLevelLoaded);
        }

        private void Start()
        {
            StartCoroutine(_levelManager.Load());
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
        }
    }
}