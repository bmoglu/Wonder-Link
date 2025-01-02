using System;
using System.Collections;
using _Content.Scripts.Grid;
using _Content.Scripts.Utilities;
using UnityEngine;

namespace _Content.Scripts.Loop
{
    public class LoopManager : MonoBehaviour
    {
        public static Action OnChipsSlideCompleted;
        public static bool LoopProcessing;

        private void Awake() => LoopProcessing = false;
        private void OnEnable() => OnChipsSlideCompleted += Loop;
        private void OnDisable() => OnChipsSlideCompleted -= Loop;
        private void Loop() => StartCoroutine(LoopRoutine());
       
        private IEnumerator LoopRoutine()
        {
            LoopProcessing = true;
            yield return null;

            yield return StartCoroutine(FillEmptyTiles());
            yield return StartCoroutine(TriggerChipsToMoveRightTile());
            
            //Check have any valid move
            GridManager.Instance.HaveValidMove();

            yield return null;
            yield return new WaitUntil(() => !GridManager.ShuffleProcessing);
            
            yield return StartCoroutine(TriggerChipsToMoveRightTile());
            LoopProcessing = false;
        }

        private static IEnumerator FillEmptyTiles()
        {
            //Fill empty tiles
            foreach (var tile in GridManager.Instance.Tiles)
            {
                if (tile.Model.Chip == null)
                {
                    GridManager.Instance.CreateChipWithUpperOfColumn(tile);
                    yield return null;
                }
            }
        }

        private static IEnumerator TriggerChipsToMoveRightTile()
        {
            foreach (var tile in GridManager.Instance.Tiles)
            {
                if(tile.Model.Chip != null) tile.Model.Chip.Model.OnMoveToTile?.Invoke();
                yield return null;
            }
            
            yield return Helpers.GetWaitForSeconds(GlobalValue.GlobalAnimationDuration);
        }
        
    }
}