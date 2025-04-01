using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// 能否使用对象池的对象接口
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// 当对象生成时
        /// </summary>
        void OnObjectSpawn();
    
        /// <summary>
        /// 当对象回收时
        /// </summary>
        void OnObjectDespawn();
    }

    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : UnityEngine.Component, IPoolable
    {
        private Queue<T> _pool = new Queue<T>();
        private T prefab;

        public ObjectPool(T prefab, int initialSize = 5)
        {
            this.prefab = prefab;
            for (int i = 0; i < initialSize; i++)
            {
                T newItem = Object.Instantiate(prefab);
                newItem.gameObject.SetActive(false);
                _pool.Enqueue(newItem);
            }
        }

        /// <summary>
        /// 获取对象，池中没有缓存时，则实例化新对象
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            T item;
            if (_pool.Count == 0)
            {
                item = Object.Instantiate(prefab);
            }
            else
            {
                item = _pool.Dequeue();
            }
        
            item.OnObjectSpawn();
            item.gameObject.SetActive(true);
            return item;
        }

        /// <summary>
        /// 回收对象到池中
        /// </summary>
        /// <param name="item"></param>
        public void ReturnToPool(T item)
        {
            item.OnObjectDespawn();
            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
        }
    }
}