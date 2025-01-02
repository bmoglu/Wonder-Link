using _Content.Scripts.Chip;
using UnityEngine;

namespace _Content.Scripts.Grid.Tile
{
    public class TileController : MonoBehaviour
    {
        public TileModel Model;

        public void Initialize(Vector2Int index,Vector2 worldPosition,Transform parent)
        {
            Model = new TileModel(this,index);
            
            gameObject.name = $"Tile {index.x},{index.y}";
            transform.position = worldPosition;
            transform.SetParent(parent);

            Model.OnTileClear += TileClear;
        }

        private void OnDisable()
        {
            if (Model != null)
            {
                Model.OnTileClear -= TileClear;
            }
        }

        private void TileClear()
        {
            ChipController chip = Model.ReleaseChip();
            chip.Model.OnClear?.Invoke();
        }
        
    }
}