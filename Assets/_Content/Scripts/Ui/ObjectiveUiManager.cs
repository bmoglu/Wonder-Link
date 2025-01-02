using System;
using System.Collections.Generic;
using _Content.Scripts.Objective;
using _Content.Scripts.Patterns.Singleton;
using TMPro;
using UnityEngine;

namespace _Content.Scripts.Ui
{
    public class ObjectiveUiManager : Singleton<ObjectiveUiManager>
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI moveText;
        [SerializeField] private Transform objectiveContainer;

        public static Action OnObjectivesUpdated;
        public static Action<int> OnMovesUpdated;

        private readonly List<ObjectiveUi> _objectiveUi = new();
        
        private void OnEnable()
        {
            OnObjectivesUpdated += UpdateObjectiveUi;
            OnMovesUpdated += MovesUpdated;
        }
        
        private void OnDisable()
        {
            OnObjectivesUpdated -= UpdateObjectiveUi;
            OnMovesUpdated -= MovesUpdated;
        }

        public void Initialize(ObjectiveConfig objectiveConfig)
        {
            foreach (var objective in objectiveConfig.Objectives)
            {
                var newObjectiveUi = FactoryManager.FactoryManager.Instance.Create<ObjectiveUi>();
                newObjectiveUi.Initialize(objective,objectiveContainer);
                _objectiveUi.Add(newObjectiveUi);
            }
            
            moveText.SetText(objectiveConfig.MoveCount.ToString());
        }

        private void UpdateObjectiveUi()
        {
            foreach (var ui in _objectiveUi)
            {
                ui.OnObjectiveUpdated?.Invoke();
            }
        }
        
        private void MovesUpdated(int moves)
        {
            moveText.SetText(moves.ToString());
        }
    }
}