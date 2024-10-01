using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.CharacterScripts
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private List<CheckPoint> checkPoints;
        [SerializeField] private PlayerHealthManager playerHealthManager;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Vector3 lastCheckPoint;
        [SerializeField] private bool isSpawn;

        private void OnEnable()
        {
            playerHealthManager.OnPlayerRespawn += SpawnPlayerOnLastCheckPoint;
            foreach (var checkPoint in checkPoints)
            {
                checkPoint.OnCheckPointEnter += SaveCheckPointTransform;
            }
        }

        private void FixedUpdate()
        {
            if (!isSpawn) return;
            SpawnOnCheckPoint();
            isSpawn = false;
        }

        private void OnDisable()
        {
            playerHealthManager.OnPlayerRespawn -= SpawnPlayerOnLastCheckPoint;
            foreach (var checkPoint in checkPoints)
            {
                checkPoint.OnCheckPointEnter -= SaveCheckPointTransform;
            }
        }

        private void SaveCheckPointTransform(Vector3 lastSpawnPoint)
        {
            lastCheckPoint = lastSpawnPoint;
        }

        private void SpawnPlayerOnLastCheckPoint()
        {
            StartCoroutine(SpawnPlayerTimer());
        }

        private IEnumerator SpawnPlayerTimer()
        {
           
            yield return new WaitForSecondsRealtime(0.5f);
            isSpawn = true;
            playerHealthManager.PlayerDissolve.AppearPlayer();
            playerHealthManager.BotInput.enabled = true;
        }
        
        private void SpawnOnCheckPoint()
        {
            playerTransform.position = lastCheckPoint;
        }
    }
}