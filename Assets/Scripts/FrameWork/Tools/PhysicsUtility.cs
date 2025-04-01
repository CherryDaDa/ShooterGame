using UnityEngine;

namespace Framework.Tools
{
    public static class PhysicsUtility
    {
        public static bool CheckOverlapSphere(Vector3 position, float radius, int layerMask)
        {
            return Physics.CheckSphere(position, radius, layerMask);
        }

        // ... 其他物理相关方法
    }
}