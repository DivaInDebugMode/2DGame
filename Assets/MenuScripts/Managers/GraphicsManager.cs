using System;
using Graphics;
using Settings;
using UnityEngine;

namespace Managers
{
    public class GraphicsManager : MonoBehaviour
    {
        [SerializeField] private GraphicsIndex index;
        public GraphicsIndex Index => index;

        private void Awake()
        {
            ChangeGraphics();
        }

        private void ChangeGraphics()
        {
            QualitySettings.SetQualityLevel(index.Index);
        }
    }
}