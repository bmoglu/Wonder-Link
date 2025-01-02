using System;
using _Content.Scripts.Enum;
using _Content.Scripts.Grid.Tile;

namespace _Content.Scripts.Chip
{
    public class ChipModel
    {
        public TileController Tile { get; set; }
        public ColorType Type { get; }

        public Action OnClear;
        public Action OnMoveToTile;
        public Action<bool> OnHighlight;
        
        public ChipModel(ColorType type)
        {
            Type = type;
        }
    }
}