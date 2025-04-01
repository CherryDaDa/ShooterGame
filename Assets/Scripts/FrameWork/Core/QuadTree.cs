using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// 可以进行比较的二维坐标点
    /// </summary>
    public interface IComparableCoordinates
    {
        Vector2 Coordinates { get; }
    }
    
    /// <summary>
    /// 四叉树区域类型
    /// </summary>
    public enum MapRegion
    {
        None = -1,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
    
    /// <summary>
    /// 区域信息
    /// </summary>
    public class RegionInfo
    {
        public MapRegion Region;
        public Rect Bounds;
    }
    
    /// <summary>
    /// 四叉树类，用于空间分区和快速对象查询。
    /// </summary>
    public class QuadTree<T> where T : IComparableCoordinates
    {
        /// <summary>
        /// 获取或设置四叉树的边界。
        /// </summary>
        public Rect Bound { get; }

        /// <summary>
        /// 每个象限的最大容量，超出后增加新的节点深度，直到达到maxLevels
        /// </summary>
        private readonly int maxObjects = 10;
        
        /// <summary>
        /// 树的最大深度
        /// </summary>
        private readonly int maxLevels = 5;

        private readonly int _level;
        private readonly List<T> _objects;
        private Rect _bounds;
        private readonly QuadTree<T>[] _nodes;

        /// <summary>
        /// 构造函数，初始化四叉树节点。
        /// </summary>
        /// <param name="level">节点层级。</param>
        /// <param name="bounds">节点边界。</param>
        public QuadTree(int level, Rect bounds)
        {
            this._level = level;
            this._objects = new List<T>();
            this._bounds = bounds;
            this._nodes = new QuadTree<T>[4];

            Bound = this._bounds;
        }
        
        public QuadTree(int level, Rect bounds, int maxObject, int maxLevel)
        {
            this._level = level;
            this._objects = new List<T>();
            this._bounds = bounds;
            this._nodes = new QuadTree<T>[4];

            maxObjects = maxObject;
            maxLevels = maxLevel;

            Bound = this._bounds;
        }

        /// <summary>
        /// 清除当前节点及其子节点的所有对象。
        /// </summary>
        public void Clear()
        {
            _objects.Clear();

            for (int i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i] != null)
                {
                    _nodes[i].Clear();
                    _nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// 将当前节点分割为四个子节点。
        /// </summary>
        private void Split()
        {
            float subWidth = _bounds.width / 2.0f;
            float subHeight = _bounds.height / 2.0f;
            float x = _bounds.x;
            float y = _bounds.y;

            _nodes[0] = new QuadTree<T>(_level + 1, new Rect(x + subWidth, y, subWidth, subHeight));
            _nodes[1] = new QuadTree<T>(_level + 1, new Rect(x, y, subWidth, subHeight));
            _nodes[2] = new QuadTree<T>(_level + 1, new Rect(x, y + subHeight, subWidth, subHeight));
            _nodes[3] = new QuadTree<T>(_level + 1, new Rect(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        /// <summary>
        /// 确定目标对象应该放在哪个子节点中。
        /// </summary>
        /// <param name="obj">目标对象。</param>
        /// <returns>子节点索引。</returns>
        public int GetIndex(T obj)
        {
            // 判断对象是否在四叉树范围内
            if (!_bounds.Contains(obj.Coordinates))
            {
                return -1;
            }

            double verticalMidpoint = _bounds.x + (_bounds.width / 2.0);
            double horizontalMidpoint = _bounds.y + (_bounds.height / 2.0);

            // 在这里根据 T 类型的比较规则来确定放入哪个象限
            bool topQuadrant = obj.Coordinates.y >= horizontalMidpoint;
            bool bottomQuadrant = obj.Coordinates.y < horizontalMidpoint;

            if (obj.Coordinates.x < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    return 0;
                }
                else if (bottomQuadrant)
                {
                    return 2;
                }
            }
            else if(obj.Coordinates.x < verticalMidpoint + _bounds.width)// if (obj.Coordinates.x > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    return 1;
                }
                else if (bottomQuadrant)
                {
                    return 3;
                }
            }

            return -1; // 不在任何象限内
        }
        
        public MapRegion GetRegion(T obj)
        {
            int index = GetIndex(obj);
            return (MapRegion)index;
        }

        /// <summary>
        /// 将对象插入到四叉树中。
        /// </summary>
        /// <param name="obj">要插入的对象。</param>
        public void Insert(T obj)
        {
            if (_nodes[0] != null)
            {
                int index = GetIndex(obj);

                if (index != -1)
                {
                    _nodes[index].Insert(obj);
                    return;
                }
            }

            _objects.Add(obj);

            if (_objects.Count > maxObjects && _level < maxLevels)
            {
                if (_nodes[0] == null)
                {
                    Split();
                }

                int i = 0;
                while (i < _objects.Count)
                {
                    int index = GetIndex(_objects[i]);
                    if (index != -1)
                    {
                        _nodes[index].Insert(_objects[i]);
                        _objects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// 返回与给定对象相交的所有对象。
        /// </summary>
        /// <param name="returnObjects">用于存储结果的列表。</param>
        /// <param name="obj">查询的对象。</param>
        /// <returns>相交的对象列表。</returns>
        public List<T> Retrieve(List<T> returnObjects, T obj)
        {
            int index = GetIndex(obj);
            if (index != -1 && _nodes[0] != null)
            {
                _nodes[index].Retrieve(returnObjects, obj);
            }

            returnObjects.AddRange(_objects);

            return returnObjects;
        }
        
        /// <summary>
        /// 返回给定对象所在的区域信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public RegionInfo GetRegionInfo(T obj)
        {
            int index = GetIndex(obj);
            return index != -1 ? new RegionInfo
            {
                Region = (MapRegion)index,
                Bounds = CalculateBoundsForRegion((MapRegion)index)
            } : null;
        }

        /// <summary>
        /// 计算区域范围
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private Rect CalculateBoundsForRegion(MapRegion region)
        {
            // 根据实际情况计算每个子区域的边界
            // 这里简单演示，具体根据实际需求调整
            switch (region)
            {
                case MapRegion.TopLeft:
                    return new Rect(_bounds.x, _bounds.y + _bounds.height / 2, _bounds.width / 2, _bounds.height / 2);
                case MapRegion.TopRight:
                    return new Rect(_bounds.x + _bounds.width / 2, _bounds.y + _bounds.height / 2, _bounds.width / 2, _bounds.height / 2);
                case MapRegion.BottomLeft:
                    return new Rect(_bounds.x, _bounds.y, _bounds.width / 2, _bounds.height / 2);
                case MapRegion.BottomRight:
                    return new Rect(_bounds.x + _bounds.width / 2, _bounds.y, _bounds.width / 2, _bounds.height / 2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
