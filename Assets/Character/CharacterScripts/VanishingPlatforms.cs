using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class VanishingPlatforms : MonoBehaviour
    {
        [SerializeField] private int platformIndex;
        [SerializeField] private LayerMask playerLayer;
        public event Action<int> OnPlayerDetected;

        private void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & playerLayer) == 0) return;
            OnPlayerDetected?.Invoke(platformIndex);
        }
    }
 
    
}