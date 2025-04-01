using System;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target; // 要跟随的目标

    public bool followX = true; // 是否跟随X轴
    public bool followY = true; // 是否跟随Y轴
    public bool followZ = true; // 是否跟随Z轴

    public float offsetX = 0.0f; // X轴的跟随距离
    public float offsetY = 0.0f; // Y轴的跟随距离
    public float offsetZ = 0.0f; // Z轴的跟随距离

    private void LateUpdate()
    {
        if(target == null) 
        {
            return;
        }

        Vector3 newPosition = transform.position;
        
        if (followX) 
        {
            newPosition.x = target.position.x + offsetX;
        }
        if (followY) 
        {
            newPosition.y = target.position.y + offsetY;
        }
        if (followZ) 
        {
            newPosition.z = target.position.z + offsetZ;
        }

        transform.position = newPosition;
    }
}