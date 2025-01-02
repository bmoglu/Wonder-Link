using System;
using _Content.Scripts.Chip;
using UnityEngine;

namespace _Content.Scripts.Grid.Tile
{
    public class TileModel
    {
        public ChipController Chip { get; private set; }
        public Vector2Int Index { get; }
        
        public Action OnTileClear;
        
        private readonly TileController Tile;
        
        public TileModel(TileController tile,Vector2Int index)
        {
            Tile = tile;
            Index = index;
        }
        
        public void SetChip(ChipController chip)
        {
            Chip = chip;
            chip.Model.Tile = Tile;
        }

        public ChipController ReleaseChip()
        {
            ChipController chip = Chip;
            chip.Model.Tile = null;

            Chip = null;
            return chip;
        }
        
    }
}