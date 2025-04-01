using UnityEngine;

namespace Framework.Component.Gizmos
{
    public class ShowBounds : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            // 获取对象的包围盒
            Bounds bounds = GetComponentInChildren<Renderer>().bounds;

            // 设置Gizmos的颜色
            UnityEngine.Gizmos.color = Color.yellow;

            // 在场景中绘制包围盒
            UnityEngine.Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}