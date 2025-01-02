using System.Collections.Generic;
using _Content.Scripts.Grid.Tile;
using UnityEngine;

namespace _Content.Scripts.Grid
{
    public static class GridHelper
    {
        #region Public Methods

        public static Vector2 CalculateCenterGridPosition(GridConfig gridConfig)
        {
            float gridWidth = (gridConfig.Columns - 1) * gridConfig.TileSpacing;
            float gridHeight = (gridConfig.Rows - 1) * gridConfig.TileSpacing;

            return new Vector2(-gridWidth / 2, -gridHeight / 2);
        }
        
        public static TileController GetTileAt(TileController[,] tiles, int x, int y)
        {
            return tiles[x,y];
        }
        
        public static bool CheckValidMoves(GridConfig gridConfig,TileController[,] tiles)
        {
            for (int row = 0; row < gridConfig.Rows; row++)
            {
                for (int col = 0; col < gridConfig.Columns; col++)
                {
                    TileController tile = GetTileAt(tiles,row, col);
                    if (tile != null && tile.Model.Chip != null && CanLinkFrom(tile,gridConfig,tiles))
                    {
                        return true; // At least one move found
                    }
                }
            }
            return false; // No move
        }

        #endregion

        #region Private Methods

        private static bool IsValidTileOnGrid(GridConfig gridConfig,int x, int y)
        {
            return x >= 0 && x < gridConfig.Rows && y >= 0 && y < gridConfig.Columns;
        }

        private static List<Vector2Int> GetNeighborDirections()
        {
            return new List<Vector2Int>
            {
                new(0, 1), // Up
                new(1, 0), // Right
                new(0, -1), // Down
                new(-1, 0) // Left
            };
        }
        
        private static bool CanLinkFrom(TileController startTile,GridConfig gridConfig,TileController[,] tiles)
        {
            // Check starting tile
            if (startTile.Model.Chip == null) return false;
            var type = startTile.Model.Chip.Model.Type;

            // Linked tiles list
            List<TileController> linkedTiles = new List<TileController>();
            Queue<TileController> queue = new Queue<TileController>();

            // Add starting tile
            queue.Enqueue(startTile);
            linkedTiles.Add(startTile);

            // Breadth-First Search (BFS) algorithm check with neighbours
            while (queue.Count > 0)
            {
                TileController currentTile = queue.Dequeue();

                foreach (Vector2Int direction in GetNeighborDirections())
                {
                    int neighborRow = currentTile.Model.Index.x + direction.x;
                    int neighborCol = currentTile.Model.Index.y + direction.y;

                    if (IsValidTileOnGrid(gridConfig,neighborRow, neighborCol))
                    {
                        TileController neighborTile = GetTileAt(tiles,neighborRow, neighborCol);

                        // if neighbour same type and not in the list add to list
                        if (neighborTile != null && neighborTile.Model.Chip != null &&
                            neighborTile.Model.Chip.Model.Type == type &&
                            !linkedTiles.Contains(neighborTile))
                        {
                            linkedTiles.Add(neighborTile);
                            queue.Enqueue(neighborTile);
                        }
                    }
                }
            }

            // if linked tile more than 3 then return
            return linkedTiles.Count >= 3;
        }

        #endregion
    }
}