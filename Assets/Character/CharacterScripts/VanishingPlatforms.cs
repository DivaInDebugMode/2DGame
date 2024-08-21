using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class VanishingPlatforms : MonoBehaviour
    {
        [SerializeField] private int platformIndex;
        [SerializeField] private LayerMask playerLayer;
        public event Action<int> OnPlayerDetected;

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & playerLayer) == 0) return;
            OnPlayerDetected?.Invoke(platformIndex);
        }
        
    }
  
}

