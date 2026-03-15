using System;
using CardMatch.Data;
using CardMatch.Events;
using CardMatch.SO;
using TMPro;
using UnityEngine;

namespace CardMatch.Managers
{
    public class GameUIManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private OnLevelLoaded onLevelLoaded;
        [SerializeField] private OnScoreUpdated onScoreUpdated;

        [Header("UI Components")] [SerializeField]
        private TextMeshProUGUI scoreLabel;
        [SerializeField] private TextMeshProUGUI levelLabel;

        private void OnEnable()
        {
            onLevelLoaded.Subscribe(OnLevelLoaded);
            onScoreUpdated.Subscribe(OnScoreUpdated);
        }

        private void OnDisable()
        {
            onLevelLoaded.Unsubscribe(OnLevelLoaded);
            onScoreUpdated.Unsubscribe(OnScoreUpdated);
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