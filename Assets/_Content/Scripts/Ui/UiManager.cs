using System;
using _Content.Scripts.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Content.Scripts.Ui
{
    public class UiManager : MonoBehaviour
    {
        [Header("Win")]
        [SerializeField] private GameObject winPanel;
        [SerializeField] private Button winButton;
        [Header("Lose")]
        [SerializeField] private GameObject losePanel;
        [SerializeField] private Button loseButton;

        public static Action<GameStatus> OnGameStatusUiUpdated;
        
        private void OnEnable()
        {
            winButton.onClick.AddListener(OnButtonClicked);
            loseButton.onClick.AddListener(OnButtonClicked);
            
            OnGameStatusUiUpdated += GameStatusUiUpdated;
        }

        private void OnDisable()
        {
            winButton.onClick.RemoveListener(OnButtonClicked);
            loseButton.onClick.RemoveListener(OnButtonClicked);
            
            OnGameStatusUiUpdated -= GameStatusUiUpdated;
        }
        
        private static void OnButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void GameStatusUiUpdated(GameStatus gameStatus)
        {
            switch (gameStatus)
            {
                case GameStatus.Win:
                    winPanel.SetActive(true);
                    break;
                case GameStatus.Lose:
                    losePanel.SetActive(true);
                    break;
            }
        }
    }
}
