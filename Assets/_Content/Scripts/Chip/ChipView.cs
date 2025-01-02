using _Content.Scripts.Utilities;
using DG.Tweening;
using UnityEngine;

namespace _Content.Scripts.Chip
{
    public class ChipView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private ChipModel _chipModel;

        public void Initialize(ChipModel chipModel)
        {
            _chipModel = chipModel;

            _chipModel.OnMoveToTile += MoveToTile;
            _chipModel.OnHighlight += Highlight;
            
            SetSprite();
        }
        
        private void OnDisable()
        {
            if (_chipModel != null)
            {
                _chipModel.OnMoveToTile -= MoveToTile;    
                _chipModel.OnHighlight -= Highlight;
            }
        }

        private void SetSprite()
        {
            spriteRenderer.sprite = ChipSpriteHandler.GetSprite(_chipModel.Type);
        }

        private void MoveToTile()
        {
            if (transform.position != _chipModel.Tile.transform.position)
            {
                transform.MoveExtension(_chipModel.Tile.transform.position,GlobalValue.GlobalAnimationDuration,Ease.OutBack);    
            }
        }
        
        private void Highlight(bool state)
        {
            if (state)
            {
                transform.ScaleExtension(Vector3.one * 1.2f,GlobalValue.GlobalAnimationDuration / 2f);
                ChipLinkHandler.OnAddChip?.Invoke(_chipModel.Type,transform.position);
            }
            else
            {
                transform.ScaleExtension(Vector3.one,GlobalValue.GlobalAnimationDuration / 2f,Ease.InBack);
                ChipLinkHandler.OnRemoveChip?.Invoke();
            }
        }
    }
}