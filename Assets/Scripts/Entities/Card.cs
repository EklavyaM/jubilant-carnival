using System;
using System.Collections;
using CardMatch.SO;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.Entities
{
    public class Card : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private Image image;

        [SerializeField] private Button button;

        [Header("Properties")] [SerializeField]
        private GameData gameData;

        [Header("Test")] [SerializeField] private Sprite testFront;
        [SerializeField] private bool testMode;

        private Sprite _frontSprite;

        public Sprite FrontSprite
        {
            set => _frontSprite = value;
            get => testMode ? testFront : _frontSprite;
        }

        public bool IsFlipping { private set; get; }
        public bool IsFront { private set; get; } = false;

        public event Action<Card> OnClick;

        private void Awake()
        {
            button.onClick.AddListener(Clicked);
        }

        private void OnEnable()
        {
            image.sprite = IsFront ? FrontSprite : gameData.BackSprite;
        }

        private void Clicked()
        {
            Flip();

            if (IsFlipping) return;
            OnClick?.Invoke(this);
        }

        public void Flip()
        {
            if (!IsFlipping)
                StartCoroutine(FlipCoroutine());
        }

        IEnumerator FlipCoroutine()
        {
            IsFlipping = true;

            float time = 0f;

            while (time < gameData.FlipDuration * 0.5f)
            {
                float t = time / (gameData.FlipDuration * 0.5f);
                float scaleX = Mathf.Lerp(1f, 0f, t);
                transform.localScale = new Vector3(scaleX, 1f, 1f);

                time += Time.deltaTime;
                yield return null;
            }

            transform.localScale = new Vector3(0f, 1f, 1f);

            IsFront = !IsFront;
            image.sprite = IsFront ? FrontSprite : gameData.BackSprite;

            time = 0f;

            while (time < gameData.FlipDuration * 0.5f)
            {
                float t = time / (gameData.FlipDuration * 0.5f);
                float scaleX = Mathf.Lerp(0f, 1f, t);
                transform.localScale = new Vector3(scaleX, 1f, 1f);

                time += Time.deltaTime;
                yield return null;
            }

            transform.localScale = new Vector3(1f, 1f, 1f);

            IsFlipping = false;
        }
    }
}