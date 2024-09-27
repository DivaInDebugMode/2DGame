using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform_Scripts
{
    public class PlatformManager : MonoBehaviour
    {
        [SerializeField] private List<VanishPlatform> vanishPlatforms;

        private void OnEnable()
        {
            foreach (var platform in vanishPlatforms)
            {
                platform.OnPlayerDetection += SetActivePlatform;
            }
        }

        private void OnDisable()
        {
            foreach (var platform in vanishPlatforms)
            {
                platform.OnPlayerDetection -= SetActivePlatform;
            }
        }

        private void SetActivePlatform(int index)
        {
            StartCoroutine(DeactivatePlatformTimer(index));
        }

        private IEnumerator DeactivatePlatformTimer(int index)
        {
            yield return new WaitForSecondsRealtime(vanishPlatforms[index].DisappearTime);
            vanishPlatforms[index].gameObject.SetActive(false);
            StartCoroutine(SetActivePlatformTimer(index));
        }

        private IEnumerator SetActivePlatformTimer(int index)
        {
            yield return new WaitForSecondsRealtime(vanishPlatforms[index].SpawnTime);
            vanishPlatforms[index].gameObject.SetActive(true);
        }
    }
}