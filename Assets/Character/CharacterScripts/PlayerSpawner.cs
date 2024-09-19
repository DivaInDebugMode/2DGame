using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private List<CheckPoint> checkPoints;
        [SerializeField] private PlayerHealthManager playerHealthManager;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Vector3 lastCheckPoint;

        private void OnEnable()
        {
            foreach (var checkPoint in checkPoints)
            {
                checkPoint.OnCheckPointEnter += SaveCheckPointTransform;
            }

            playerHealthManager.OnPlayerDeath += SpawnPlayerOnLastCheckPoint;
        }

        private void OnDisable()
        {
            foreach (var checkPoint in checkPoints)
            {
                checkPoint.OnCheckPointEnter -= SaveCheckPointTransform;
            }
            playerHealthManager.OnPlayerDeath -= SpawnPlayerOnLastCheckPoint;
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
            yield return new WaitForSecondsRealtime(1f);
            playerTransform.position = lastCheckPoint;
        }
    }
}