using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BouncePlatform : MonoBehaviour
    {
        [SerializeField] private float bounceHeight;
        [SerializeField] private LayerMask playerLayer;
        private void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & playerLayer) == 0) return;
            collision.rigidbody.AddForce(Vector3.up * bounceHeight,ForceMode.Impulse);
        }
    }
}