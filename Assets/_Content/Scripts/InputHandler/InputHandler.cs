using System;
using System.Collections.Generic;
using _Content.Scripts.Chip;
using _Content.Scripts.Enum;
using _Content.Scripts.Grid;
using _Content.Scripts.Grid.Tile;
using _Content.Scripts.Loop;
using _Content.Scripts.Objective;
using UnityEngine;

namespace _Content.Scripts.InputHandler
{
    public class InputHandler : MonoBehaviour
    {
        public static Action OnValidMove;
        
        private readonly List<TileController> _linkedTiles = new(); 
        private TileController _currentTile;
        private Camera _mainCamera;
        
        private bool _isStarted;
        
        private void Awake() => _mainCamera = Camera.main;
       
        private void Update()
        {
            if (!LoopManager.LoopProcessing && GameControl.GameStatus == GameStatus.Playing) HandleInput();
        }
        
        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0) && !_isStarted) StartInput();
            if (Input.GetMouseButton(0)) ContinueInput();
            if (Input.GetMouseButtonUp(0) && _isStarted) EndInput();
        }

        private void StartInput()
        {
            _isStarted = true;
            
            // Get first tile
            _currentTile = CastToTile();
            
            if (_currentTile != null)
            {
                _linkedTiles.Clear();
                _linkedTiles.Add(_currentTile);
                HighlightTile(_currentTile, true);
            }
        }

        private void ContinueInput()
        {
            if (_currentTile == null) return;
            
            TileController newTile = CastToTile();
            if (newTile == null || newTile == _currentTile || (newTile.Model.Chip != null && newTile.Model.Chip.Model.Type != _currentTile.Model.Chip.Model.Type) ) return;

            //Back control
            if (_linkedTiles.Count > 1 && newTile == _linkedTiles[^2])
            {
                HighlightTile(_linkedTiles[^1], false);
                _linkedTiles.RemoveAt(_linkedTiles.Count - 1);
                _currentTile = newTile;
                return;
            }

            if (IsValidConnection(newTile) && !_linkedTiles.Contains(newTile))
            {
                _currentTile = newTile;
                _linkedTiles.Add(_currentTile);
                HighlightTile(_currentTile, true);
            }
        }
    
        private void EndInput()
        {
            // Unhighlight tiles
            foreach (var tile in _linkedTiles)
            {
                HighlightTile(tile, false);
            }
            
            if (_linkedTiles.Count >= 3)
            {
                // Process link tiles
                ProcessSelection(_linkedTiles);
            
                OnValidMove?.Invoke();
            }
            
            _linkedTiles.Clear();
            _currentTile = null;
            _isStarted = false; 
        }

        private TileController CastToTile()
        {
            Vector2 worldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector3.forward);

            if (hit.collider != null)
            {
                return hit.collider.GetComponent<TileController>();
            }

            return null;
        }

        private bool IsValidConnection(TileController targetTile)
        {
            // Check if the target tile has a chip and matches the current tile's chip type
            if (targetTile.Model.Chip == null || targetTile.Model.Chip.Model.Type != _currentTile.Model.Chip.Model.Type)
                return false;

            // Calculate the row and column differences
            int rowDiff = Mathf.Abs(targetTile.Model.Index.x - _currentTile.Model.Index.x);
            int colDiff = Mathf.Abs(targetTile.Model.Index.y - _currentTile.Model.Index.y);

            // Check if the tiles are neighbors (directly adjacent)
            return (rowDiff == 1 && colDiff == 0) || (rowDiff == 0 && colDiff == 1);
        }

        private static void HighlightTile(TileController tile, bool highlight)
        {
            tile.Model.Chip.Model.OnHighlight?.Invoke(highlight);
        }

        private static void ProcessSelection(List<TileController> tiles)
        {
            var colorType = tiles[0].Model.Chip.Model.Type;
        
            // Clear tiles or chips
            foreach (var tile in tiles)
            {
                tile.Model.OnTileClear?.Invoke();
            }
        
            GridManager.Instance.OnSlideChips?.Invoke();
            ObjectiveManager.OnCheckObjectiveCompleted?.Invoke(colorType,tiles.Count);
        }
    }
}
