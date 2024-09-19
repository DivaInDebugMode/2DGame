using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class CollisionDetection : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayerMask;
        [SerializeField] private int damageAmount;
        public event Action<int> OnPlayerDetection;
        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & playerLayerMask.value) != 0)
            {
                OnPlayerDetection?.Invoke(damageAmount);
            }
        }
    }
}