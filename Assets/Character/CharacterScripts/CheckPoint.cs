using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class CheckPoint : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayerMask;
        public event Action<Vector3> OnCheckPointEnter;
        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & playerLayerMask.value) != 0)
            {
                OnCheckPointEnter?.Invoke(transform.position);
            }
        }
    }
}