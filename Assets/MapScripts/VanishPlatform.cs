using System;
using UnityEngine;

namespace Platform_Scripts
{
    public class VanishPlatform : MonoBehaviour
    {
        [SerializeField] private int platformIndex;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float spawnTime;
        [SerializeField] private float disappearTime;
        public event Action<int> OnPlayerDetection;
        public float SpawnTime => spawnTime;
        public float DisappearTime => disappearTime;
        
        private void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & playerLayer) == 0) return;
            OnPlayerDetection?.Invoke(platformIndex);
        }
    }
}