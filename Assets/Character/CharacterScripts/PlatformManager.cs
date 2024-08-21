using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class PlatformManager : MonoBehaviour
    {
        [SerializeField] private List<VanishingPlatforms> platformsList = new();

        private void OnEnable()
        {
            foreach (var platform in platformsList)
            {
                platform.OnPlayerDetected += HandlePlatformVanish;
            }
        }

        private void OnDisable()
        {
            foreach (var platform in platformsList)
            {
                platform.OnPlayerDetected -= HandlePlatformVanish;
            }
        }
        private void HandlePlatformVanish(int index)
        {
            StartCoroutine(CubeDisappearingTimer(index));
        }
        private IEnumerator CubeDisappearingTimer(int index)
        {
            yield return new WaitForSecondsRealtime(1f);
            platformsList[index].gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(10f); 
            platformsList[index].gameObject.SetActive(true);
        }
    }
}