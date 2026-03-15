using System;
using CardMatch.Data;
using CardMatch.Events;
using CardMatch.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.Managers
{
    public class GameUIManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private OnLevelLoaded onLevelLoaded;
        [SerializeField] private OnScoreUpdated onScoreUpdated;
        [SerializeField] private OnReturnToMenu onReturnToMenu;

        [Header("UI Components")] [SerializeField]
        private TextMeshProUGUI scoreLabel;
        [SerializeField] private TextMeshProUGUI levelLabel;
        [SerializeField] private Button returnToMenu;

        private void OnEnable()
        {
            onLevelLoaded.Subscribe(OnLevelLoaded);
            onScoreUpdated.Subscribe(OnScoreUpdated);
            
            returnToMenu.onClick.AddListener(onReturnToMenu.Raise);
        }

        private void OnDisable()
        {
            onLevelLoaded.Unsubscribe(OnLevelLoaded);
            onScoreUpdated.Unsubscribe(OnScoreUpdated);
            
            returnToMenu.onClick.RemoveListener(onReturnToMenu.Raise);
        }

        private void OnLevelLoaded(LevelData level)
        {
            levelLabel.text = $"Level : {level.layoutData.x} x {level.layoutData.y}";
        }

        private void OnScoreUpdated(ScoreData score)
        {
            scoreLabel.text = $"Matches : {score.matches} / {score.totalMatches}";
        }
    }
}