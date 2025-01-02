using System;
using System.Collections.Generic;
using System.Linq;
using _Content.Scripts.Chip;
using _Content.Scripts.Enum;
using _Content.Scripts.Grid.Tile;
using _Content.Scripts.Loop;
using _Content.Scripts.Patterns.Singleton;
using UnityEngine;

namespace _Content.Scripts.Grid
{
    public class GridManager : Singleton<GridManager>
    {
        [Header("Grid Configuration")]
        [SerializeField] private GridConfig gridConfig;
        
        public TileController[,] Tiles { get; private set; }
        
        public Action OnSlideChips;
        public static bool ShuffleProcessing;
        
        private readonly System.Random _random = new();

        #region Initialize

        private void Awake()
        {
            CreateGrid();
            InitializeValidMove();
        }

        private void OnEnable() => OnSlideChips += SlideChips;
        private void OnDisable() => OnSlideChips -= SlideChips;
        
        private void CreateGrid()
        {
            if (gridConfig == null)
            {
                Debug.LogError("GridConfig is not assigned!");
                return;
            }

            Tiles = new TileController[gridConfig.Rows, gridConfig.Columns];

            for (int row = 0; row < gridConfig.Rows; row++)
            {
                for (int col = 0; col < gridConfig.Columns; col++)
                {
                    //Create Tile
                    TileController newTile = FactoryManager.FactoryManager.Instance.Create<TileController>();

                    newTile.Initialize(
                        new Vector2Int(row, col),
                        new Vector2(col * gridConfig.TileSpacing, row * gridConfig.TileSpacing),
                        transform);

                    Tiles[row, col] = newTile;
                }
            }

            transform.position = GridHelper.CalculateCenterGridPosition(gridConfig);
            
            InitializeTilesWithChips();
        }
        
        private void InitializeTilesWithChips()
        {
            foreach (TileController tile in Tiles)
            {
                CreateChip(tile);
            }
        }
        
        private void InitializeValidMove()
        {
            if (!GridHelper.CheckValidMoves(gridConfig, Tiles))
            {
                InitializeShuffleGrid();
            }
        }
        
        private void InitializeShuffleGrid()
        {
            // Get all chips to list
            List<ChipController> allChips = new List<ChipController>();
            foreach (TileController tile in Tiles)
            {
                if (tile != null && tile.Model.Chip != null)
                {
                    allChips.Add(tile.Model.Chip);
                    tile.Model.ReleaseChip();
                }
            }

            // Order chip with random numbers
            allChips = allChips.OrderBy(chip => _random.Next()).ToList();

            // Assign the chips new tiles
            int index = 0;
            foreach (TileController tile in Tiles)
            {
                if (tile != null && index < allChips.Count)
                {
                    tile.Model.SetChip(allChips[index]);
                    tile.Model.Chip.transform.position = tile.transform.position;
                    index++;
                }
            }

            // No valid moves then shuffle again 
            if (!GridHelper.CheckValidMoves(gridConfig, Tiles))
            {
                InitializeShuffleGrid();
            }
        }

        #endregion

        #region Create

        private ChipController CreateChip(TileController tile)
        {
            //Select random color type
            var randomColorType = (ColorType) _random.Next(System.Enum.GetValues(typeof(ColorType)).Length);
            //Create Chip
            var chip = ChipPooler.Instance.GetChip();
            chip.Initialize(tile,randomColorType);
            
            //Set Chip
            tile.Model.SetChip(chip);
            
            return chip;
        }

        public void CreateChipWithUpperOfColumn(TileController tile)
        {
            var chip = CreateChip(tile);
            chip.transform.position = Tiles[Tiles.GetLength(0) - 1, tile.Model.Index.y].transform.position + Vector3.up;
        }

        #endregion

        #region Slide & Shuffle

        public void HaveValidMove()
        {
            ShuffleProcessing = true;
            
            if (!GridHelper.CheckValidMoves(gridConfig, Tiles))
            {
                ShuffleGrid();
            }
            else
            {
                ShuffleProcessing = false;
            }
        }
        
        private void SlideChips()
        {
            for (int col = 0; col < gridConfig.Columns; col++)
            {
                var queue = GetChipsInColumn(col);
                if(queue == null || queue.Count <= 0) continue;
                PlaceChipsInColumn(queue,col);
            }
            
            LoopManager.OnChipsSlideCompleted?.Invoke();
        }
        

        private Queue<ChipController> GetChipsInColumn(int columnIndex)
        {
            // Get chips to queue
            Queue<ChipController> chipsInColumn = new Queue<ChipController>();

            // Find chips with not null tiles
            for (int row = 0; row < gridConfig.Rows; row++)
            {
                TileController tile = GridHelper.GetTileAt(Tiles,row, columnIndex);
                if (tile.Model.Chip != null)
                {
                    ChipController chip = tile.Model.ReleaseChip();
                    chipsInColumn.Enqueue(chip);
                }
            }

            return chipsInColumn;
        }

        private void PlaceChipsInColumn(Queue<ChipController> chipsInColumn, int columnIndex)
        {
            for (int row = 0; row < gridConfig.Rows; row++)
            {
                TileController tile = GridHelper.GetTileAt(Tiles,row, columnIndex);
                    
                if (chipsInColumn.Count > 0)
                {
                    ChipController chip = chipsInColumn.Dequeue();
                    tile.Model.SetChip(chip);
                }
            }
        }
        
        private void ShuffleGrid()
        {
            // Get all chips to list
            List<ChipController> allChips = new List<ChipController>();
            foreach (TileController tile in Tiles)
            {
                if (tile != null && tile.Model.Chip != null)
                {
                    allChips.Add(tile.Model.Chip);
                    tile.Model.ReleaseChip();
                }
            }

            // Order chip with random numbers
            allChips = allChips.OrderBy(chip => _random.Next()).ToList();

            // Assign the chips new tiles
            int index = 0;
            foreach (TileController tile in Tiles)
            {
                if (tile != null && index < allChips.Count)
                {
                    tile.Model.SetChip(allChips[index]);
                    index++;
                }
            }

            // No valid moves then shuffle again 
            if (!GridHelper.CheckValidMoves(gridConfig, Tiles))
            {
                ShuffleGrid();
            }
            else
            {
                ShuffleProcessing = false;
            }
        }

        #endregion 
        
    }
}