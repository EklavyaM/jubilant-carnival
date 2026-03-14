using System;
using CardMatch.SO;
using UnityEngine;

namespace CardMatch.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private RectTransform tableRoot;

        [Header("Data")] [SerializeField] private GameData gameData;

        private LevelManager _levelManager;

        private void Awake()
        {
            _levelManager = new LevelManager(gameData, tableRoot);
        }

        private void Start()
        {
            StartCoroutine(_levelManager.Load());
        }
    }
}