using _Content.Scripts.Enum;
using _Content.Scripts.Grid.Tile;
using UnityEngine;

namespace _Content.Scripts.Chip
{
    public class ChipController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ChipView view;
        
        public ChipModel Model;

        public void Initialize(TileController tile,ColorType colorType)
        {
            Model = new ChipModel(colorType);
            view.Initialize(Model);
            
            transform.position = tile.transform.position;
            
            Model.OnClear += Clear; 
        }

        private void OnDisable()
        {
            if (Model != null) Model.OnClear -= Clear;
        }

        private void Clear()
        {
            ChipPooler.Instance.ReturnChip(this);
        }
        
    }
}