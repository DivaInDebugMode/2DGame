using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform
{
    public class DestructiblePlatform : MonoBehaviour
    {
        [SerializeField] private LayerMask player;
        [SerializeField] private List<GameObject> platforms = new();
        [SerializeField] private float activateTime;
        [SerializeField] private float deactivateTime;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if ((player & (1 << col.gameObject.layer)) != 0)
                StartCoroutine(DeactivateTimer(deactivateTime));
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if ((player & (1 << col.gameObject.layer)) != 0)
                StartCoroutine(ActivateTimer(activateTime));
        }
        

        private IEnumerator DeactivateTimer(float time)
        {
            yield return new WaitForSeconds(time);
            foreach (var platform in platforms)
                platform.SetActive(false);
        }

        private IEnumerator ActivateTimer(float time)
        { 
            yield return new WaitForSeconds(time);
            foreach (var platform in platforms)
                platform.SetActive(true);
        }

    }
}
