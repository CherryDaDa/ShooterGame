using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// 八叉树
    /// </summary>
    public class Octree
    {
        private const int MAX_OBJECTS = 10;
        private const int MAX_LEVELS = 5;

        private int level;
        private List<Bounds> objects;
        private Bounds bounds;
        private Octree[] children;

        public Octree(int level, Bounds bounds)
        {
            this.level = level;
            this.bounds = bounds;
            this.objects = new List<Bounds>(MAX_OBJECTS);
            this.children = new Octree[8];
        }

        public void Clear()
        {
            objects.Clear();

            for (int i = 0; i < 8; i++)
            {
                if (children[i] != null)
                {
                    children[i].Clear();
                    children[i] = null;
                }
            }
        }

        private void Split()
        {
            Vector3 size = bounds.size / 2.0f;
            Vector3 center = bounds.center;

            children[0] = new Octree(level + 1, new Bounds(center + new Vector3(-size.x / 2, -size.y / 2, -size.z / 2), size));
            children[1] = new Octree(level + 1, new Bounds(center + new Vector3(size.x / 2, -size.y / 2, -size.z / 2), size));
            children[2] = new Octree(level + 1, new Bounds(center + new Vector3(-size.x / 2, size.y / 2, -size.z / 2), size));
            children[3] = new Octree(level + 1, new Bounds(center + new Vector3(size.x / 2, size.y / 2, -size.z / 2), size));
            children[4] = new Octree(level + 1, new Bounds(center + new Vector3(-size.x / 2, -size.y / 2, size.z / 2), size));
            children[5] = new Octree(level + 1, new Bounds(center + new Vector3(size.x / 2, -size.y / 2, size.z / 2), size));
            children[6] = new Octree(level + 1, new Bounds(center + new Vector3(-size.x / 2, size.y / 2, size.z / 2), size));
            children[7] = new Octree(level + 1, new Bounds(center + new Vector3(size.x / 2, size.y / 2, size.z / 2), size));
        }

        public void Insert(Bounds bounds)
        {
            if (children[0] != null)
            {
                int index = GetIndex(bounds);
                if (index != -1)
                {
                    children[index].Insert(bounds);
                    return;
                }
            }

            objects.Add(bounds);

            if (objects.Count > MAX_OBJECTS && level < MAX_LEVELS && children[0] == null)
            {
                Split();

                int i = 0;
                while (i < objects.Count)
                {
                    int index = GetIndex(objects[i]);
                    if (index != -1)
                    {
                        children[index].Insert(objects[i]);
                        objects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        private int GetIndex(Bounds objBounds)
        {
            Vector3 objCenter = objBounds.center;
            bool left = objCenter.x <= bounds.center.x;
            bool right = objCenter.x > bounds.center.x;
            bool top = objCenter.y > bounds.center.y;
            bool bottom = objCenter.y <= bounds.center.y;
            bool front = objCenter.z > bounds.center.z;
            bool back = objCenter.z <= bounds.center.z;

            if (left && front && top) return 6;
            if (right && front && top) return 7;
            if (left && back && top) return 2;
            if (right && back && top) return 3;
            if (left && front && bottom) return 4;
            if (right && front && bottom) return 5;
            if (left && back && bottom) return 0;
            if (right && back && bottom) return 1;

            return -1; // Bounds spans multiple nodes
        }

        /// <summary>
        /// 返回Bounds范围内的所有对象
        /// </summary>
        /// <param name="searchBounds"></param>
        /// <returns></returns>
        public List<Bounds> Retrieve(Bounds searchBounds)
        {
            List<Bounds> returnObjects = new List<Bounds>();

            // 检查搜索区域是否与当前节点边界相交
            if (!bounds.Intersects(searchBounds))
                return returnObjects;

            // 检索当前节点中与搜索区域相交的对象
            foreach (var obj in objects)
            {
                if (searchBounds.Intersects(obj))
                    returnObjects.Add(obj);
            }

            // 递归检索相交的子节点
            if (children[0] != null)
            {
                // 遍历所有子节点
                for (int i = 0; i < 8; i++)
                {
                    // 检查子节点的边界是否与搜索区域相交
                    if (children[i].bounds.Intersects(searchBounds))
                    {
                        // 递归检索子节点并将结果添加到返回列表中
                        returnObjects.AddRange(children[i].Retrieve(searchBounds));
                    }
                }
            }

            return returnObjects;
        }
    
        // 为Gizmos绘制添加一个方法
        public void DrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            for (int i = 0; i < 8; i++)
            {
                if (children[i] != null) // 对每个子节点进行检查
                {
                    children[i].DrawGizmos();
                }
            }
        }
    }
}
