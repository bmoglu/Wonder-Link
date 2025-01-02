using System;
using System.Collections.Generic;
using _Content.Scripts.Enum;
using UnityEngine;

namespace _Content.Scripts.Chip
{
    public class ChipLinkHandler : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private LineRenderer lineRenderer;
        
        public static Action<ColorType,Vector3> OnAddChip;
        public static Action OnRemoveChip;
        
        private readonly List<Vector3> chipPositions = new();
        private readonly Color[] _colorTypeArray = {Color.red, Color.green, Color.blue, Color.yellow}; 
        private ColorType _currentColorType;
        
        private void OnEnable()
        {
            OnAddChip += AddChip;
            OnRemoveChip += RemoveChip;
        }

        private void OnDisable()
        {
            OnAddChip -= AddChip;
            OnRemoveChip -= RemoveChip;
        }

        private void AddChip(ColorType colorType,Vector3 position)
        {
            _currentColorType = colorType;
            chipPositions.Add(position);
            UpdateLineRenderer();
        }

        private void RemoveChip()
        {
            if (chipPositions.Count > 0)
            {
                chipPositions.RemoveAt(chipPositions.Count - 1);
                UpdateLineRenderer();
            }
        }

        private void UpdateLineRenderer()
        {
            lineRenderer.startColor = _colorTypeArray[(int) _currentColorType];
            lineRenderer.endColor = _colorTypeArray[(int) _currentColorType];
            lineRenderer.positionCount = chipPositions.Count;
            lineRenderer.SetPositions(chipPositions.ToArray());
        }
    }
}