using _Content.Scripts.Enum;
using UnityEngine;

namespace _Content.Scripts.Chip
{
    public static class ChipSpriteHandler
    {
        private static readonly ChipSpriteContainer ChipSprite = Resources.Load<ChipSpriteContainer>("ChipSpriteContainer");
        
        public static Sprite GetSprite(ColorType colorType)
        {
            return ChipSprite.GetSpriteByColorType(colorType);
        }
    }
}