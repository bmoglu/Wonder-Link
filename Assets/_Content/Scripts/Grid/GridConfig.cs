using UnityEngine;

namespace _Content.Scripts.Grid
{
    [CreateAssetMenu(fileName = "New GridConfig", menuName = "Configs/GridConfig")]
    public class GridConfig : ScriptableObject
    {
        [Header("Grid Settings")]
        public int Rows = 8; 
        public int Columns = 8; 
        public float TileSpacing = 1.1f;
    }

}