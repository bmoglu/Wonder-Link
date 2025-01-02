using System;
using _Content.Scripts.Enum;
using _Content.Scripts.Ui;
using UnityEngine;

namespace _Content.Scripts.Objective
{
    public class ObjectiveManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ObjectiveConfig config;
        
        public static Action<ColorType,int> OnCheckObjectiveCompleted;

        private ObjectiveConfig _objectiveConfig;
        
        private void Awake()
        {
            _objectiveConfig = config.GetRuntimeInstance();
            ObjectiveUiManager.Instance.Initialize(_objectiveConfig);
        }

        private void OnEnable()
        {
            OnCheckObjectiveCompleted += CheckObjectiveComplete;
            InputHandler.InputHandler.OnValidMove += ValidMove;
        }

        private void OnDisable()
        {
            OnCheckObjectiveCompleted -= CheckObjectiveComplete;
            InputHandler.InputHandler.OnValidMove -= ValidMove;
        }

        private void CheckObjectiveComplete(ColorType colorType,int count)
        {
            Objective objective = ObjectiveConfig.GetObjectiveByType(_objectiveConfig,colorType);
            
            if (objective != null)
            {
                objective.Score = Mathf.Clamp(objective.Score - count, 0, int.MaxValue);
                ObjectiveUiManager.OnObjectivesUpdated?.Invoke();
            }
            
            if (ObjectiveConfig.IsObjectiveCompleted(_objectiveConfig))
            {
                GameControl.OnGameStatusChanged?.Invoke(GameStatus.Win);
            }
        }

        private void ValidMove()
        {
            _objectiveConfig.MoveCount = Mathf.Clamp(--_objectiveConfig.MoveCount,0,int.MaxValue);
            ObjectiveUiManager.OnMovesUpdated?.Invoke(_objectiveConfig.MoveCount);
            
            if (_objectiveConfig.MoveCount <= 0)
            {
                GameControl.OnGameStatusChanged?.Invoke(GameStatus.Lose);
            }
        }
    }
}