using CardMatch.Data;
using CardMatch.Entities;
using CardMatch.Events;
using CardMatch.SO;
using UnityEngine;

namespace CardMatch.Managers
{
    public class ScoreManager
    {
        private readonly OnScoreUpdated _onScoreUpdated;

        private readonly ScoreData _scoreData = new ScoreData();

        public ScoreManager(OnScoreUpdated onScoreUpdated)
        {
            _onScoreUpdated = onScoreUpdated;
        }

        public void OnLevelLoaded(LevelData levelData)
        {
            _scoreData.matches = 0;
            _scoreData.totalMatches = Mathf.FloorToInt((levelData.layoutData.x * levelData.layoutData.y) * 0.5f);

            _onScoreUpdated.Raise(_scoreData);
        }

        public void OnMatchFound()
        {
            ++_scoreData.matches;
            _onScoreUpdated.Raise(_scoreData);
        }
    }
}