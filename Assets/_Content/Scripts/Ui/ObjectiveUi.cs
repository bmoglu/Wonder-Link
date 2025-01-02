using System;
using _Content.Scripts.Chip;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Content.Scripts.Ui
{
    public class ObjectiveUi : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image image; 
        [SerializeField] private TextMeshProUGUI scoreText;

        public Action OnObjectiveUpdated;
        
        private Objective.Objective _objective;
        
        public void Initialize(Objective.Objective objective,Transform parent)
        {
            _objective = objective;
        
            scoreText.SetText(objective.Score.ToString());
            image.sprite = ChipSpriteHandler.GetSprite(objective.ColorType);
        
            transform.SetParent(parent);

            OnObjectiveUpdated += ObjectiveUpdated;
        }

        private void OnDisable()
        {
            OnObjectiveUpdated -= ObjectiveUpdated;
        }

        private void ObjectiveUpdated()
        {
            scoreText.SetText(_objective.Score.ToString());
            if(_objective.Score <= 0) scoreText.gameObject.SetActive(false);
        }

    }
}
