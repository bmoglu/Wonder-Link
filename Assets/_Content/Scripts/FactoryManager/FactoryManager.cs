using System;
using _Content.Scripts.Grid.Tile;
using _Content.Scripts.Patterns.Factory;
using _Content.Scripts.Patterns.Singleton;
using _Content.Scripts.Ui;
using UnityEngine;

namespace _Content.Scripts.FactoryManager
{
    [DefaultExecutionOrder(-1000)]
    public class FactoryManager : Singleton<FactoryManager>
    {
        [Header("Prefabs")]
        [SerializeField] private TileController tilePrefab;
        [SerializeField] private ObjectiveUi objectiveUiPrefab;
        
        private IFactory<TileController> _tileFactory;
        private IFactory<ObjectiveUi> _objectiveFactory;
        
        private void Awake()
        {
            _tileFactory = new Factory<TileController>(tilePrefab);
            _objectiveFactory = new Factory<ObjectiveUi>(objectiveUiPrefab);
        }
        
        public T Create<T>()
        {
            return typeof(T) switch
            {
                var t when t == typeof(TileController) => (T) (object) _tileFactory.Create(),
                var t when t == typeof(ObjectiveUi) => (T) (object) _objectiveFactory.Create(),
                _ => throw new InvalidOperationException($"Type {typeof(T)} is not supported by the factory.")
            };
        }
    }
}