using UnityEngine;

namespace _Content.Scripts.Patterns.Factory
{
    public class Factory<T> : IFactory<T> where T : Object
    {
        private readonly T _prefab;

        public Factory(T prefab)
        {
            _prefab = prefab;
        }

        public T Create()
        {
            T newObject = Object.Instantiate(_prefab);
            return newObject;
        }
    }

}