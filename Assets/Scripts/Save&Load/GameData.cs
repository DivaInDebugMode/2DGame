using System;
using UnityEngine;

namespace Save_Load
{
    [Serializable]

    public class GameData
    {
        public Vector3 playerPos;
        public Vector3 cameraPos;

        public GameData()
        {
            playerPos = Vector3.zero;
            cameraPos = Vector3.zero;
        }
    }
}