using System;
using _Content.Scripts.Enum;
using _Content.Scripts.Ui;
using UnityEngine;

namespace _Content.Scripts
{
    public class GameControl : MonoBehaviour
    {
        public static Action<GameStatus> OnGameStatusChanged;
        public static GameStatus GameStatus;

        private void Awake() => GameStatus = GameStatus.Playing;
        private void OnEnable() => OnGameStatusChanged += GameStatusChanged;
        private void OnDisable() => OnGameStatusChanged -= GameStatusChanged;
        
        private void GameStatusChanged(GameStatus gameStatus)
        {
            if(GameStatus == GameStatus.Win || GameStatus == GameStatus.Lose) return;
            GameStatus = gameStatus;
            
            UiManager.OnGameStatusUiUpdated?.Invoke(gameStatus);
        }
    }
}