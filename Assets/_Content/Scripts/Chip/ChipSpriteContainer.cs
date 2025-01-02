using System.Collections.Generic;
using _Content.Scripts.Enum;
using UnityEngine;

namespace _Content.Scripts.Chip
{
    [CreateAssetMenu(fileName = "NewChipSpriteContainer", menuName = "Configs/ChipSpriteContainer")]
    public class ChipSpriteContainer : ScriptableObject
    {
        [Header("Sprites")]
        [SerializeField] private List<Sprite> chipSprites;

        public Sprite GetSpriteByColorType(ColorType colorType)
        {
            return chipSprites[(int)colorType];
        }
    }
}