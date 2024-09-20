﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class PlatformManager : MonoBehaviour
    {
        [SerializeField] private List<VanishingPlatforms> platformsList = new();
        [SerializeField] private float spawnTimer;
        [SerializeField] private float vanishTimer;

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
            yield return new WaitForSecondsRealtime(vanishTimer);
            platformsList[index].gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(spawnTimer); 
            platformsList[index].gameObject.SetActive(true);
        }
    }
}