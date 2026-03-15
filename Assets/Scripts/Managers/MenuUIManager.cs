using CardMatch.Events;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.Managers
{
    public class MenuUIManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button exitButton;
        
        [Header("Events")]
        [SerializeField] private OnContinue onContinue;
        [SerializeField] private OnNewGame onNewGame;
        [SerializeField] private OnExitGame onExitGame;

        private void OnEnable()
        {
            continueButton.onClick.AddListener(onContinue.Raise);
            newGameButton.onClick.AddListener(onNewGame.Raise);
            exitButton.onClick.AddListener(onExitGame.Raise);
        }

        private void OnDisable()
        {
            continueButton.onClick.RemoveListener(onContinue.Raise);
            newGameButton.onClick.RemoveListener(onNewGame.Raise);
            exitButton.onClick.RemoveListener(onExitGame.Raise);
        }
    }
}