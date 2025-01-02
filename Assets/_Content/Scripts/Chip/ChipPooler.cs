using _Content.Scripts.Patterns.ObjectPooler;
using _Content.Scripts.Patterns.Singleton;
using UnityEngine;

namespace _Content.Scripts.Chip
{
    [DefaultExecutionOrder(-990)]
    public class ChipPooler : Singleton<ChipPooler>
    {
        [Header("Prefab")]
        [SerializeField] private ChipController chipPrefab;
        
        private ObjectPooler<ChipController> _chipPool;
        
        private void Awake()
        {
            _chipPool = new ObjectPooler<ChipController>(chipPrefab,0, transform);
        }

        public ChipController GetChip()
        {
            return _chipPool.Get();
        }

        public void ReturnChip(ChipController chip)
        {
            ResetChip(chip);
            _chipPool.Return(chip);
        }

        private static void ResetChip(ChipController chip)
        {
            chip.transform.localScale = Vector3.one;
        }
    }
}