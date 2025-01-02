using System.Collections.Generic;
using UnityEngine;

namespace _Content.Scripts.Patterns.ObjectPooler
{
    public class ObjectPooler<T> where T : MonoBehaviour
    {
        private readonly Queue<T> poolQueue = new();
        private readonly T prefab;
        private readonly Transform parentTransform;

        public ObjectPooler(T prefab, int initialSize, Transform parent = null)
        {
            this.prefab = prefab;
            parentTransform = parent;

            // Create Initial Pool
            for (int i = 0; i < initialSize; i++)
            {
                T obj = Get();
                Return(obj);
            }
        }

        public T Get()
        {
            if (poolQueue.Count > 0)
            {
                T obj = poolQueue.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }
            
            T newObj = Object.Instantiate(prefab, parentTransform);
            return newObj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            poolQueue.Enqueue(obj);
        }
        
    }

}