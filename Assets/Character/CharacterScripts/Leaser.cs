using System.Collections;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class Leaser : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Vector2 startPoint;
        [SerializeField] private Vector2 hitPoint;
        [SerializeField] private float activeDuration;
        [SerializeField] private float inactiveDuration;

        private void Start()
        {
            StartCoroutine(LaserTimer());
        }

        private void FireLaser()
        {
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, hitPoint);
            lineRenderer.enabled = true;

        }

        private IEnumerator LaserTimer()
        {
            while (true)
            {
                FireLaser();
                yield return new WaitForSeconds(activeDuration);
                lineRenderer.enabled = false;
                yield return new WaitForSeconds(inactiveDuration);
            }
        }
    }
}

