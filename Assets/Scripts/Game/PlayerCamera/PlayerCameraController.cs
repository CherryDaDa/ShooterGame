using System;
using UnityEngine;

namespace Game.PlayerCamera
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private GameObject MainPlayer;

        private void Update()
        {
            transform.LookAt(MainPlayer.transform);
        }
    }
}
