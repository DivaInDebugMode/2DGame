using System;
using System.Collections;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class EdgeBoneBack : MonoBehaviour
    {
        [SerializeField] private BotData botData;
        
        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & botData.BotDetectionStats.Edge.value) != 0 && !botData.BotDetectionStats.IsOnEdgeWithSecondFoot && 
                !botData.BotDetectionStats.IsClimbingGround)
            {
                botData.BotDetectionStats.IsOnEdge = true;
                switch (botData.BotStats.CurrentDirectionValue)
                {
                    case 1:
                        botData.BotComponents.Rb.velocity = new Vector2(1.5f, -2f);
                        break;
                    case -1:
                        botData.BotComponents.Rb.velocity = new Vector2(-1.5f, -2f);
                        break;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (((1 << other.gameObject.layer) & botData.BotDetectionStats.Edge.value) != 0 &&
                !botData.BotDetectionStats.IsOnEdgeWithSecondFoot &&
                !botData.BotDetectionStats.IsClimbingGround)
            {
                botData.BotDetectionStats.IsOnEdge = true;
                switch (botData.BotStats.CurrentDirectionValue)
                {
                    case 1:
                        if (botData.BotComponents.Rb.velocity == Vector3.zero)
                        {
                            botData.BotComponents.Rb.velocity = new Vector2(2f, -2f);
                        }
                        
                        break;
                    case -1:
                        if (botData.BotComponents.Rb.velocity == Vector3.zero)
                        {
                            botData.BotComponents.Rb.velocity = new Vector2(-2f, -2f);
                        }
                        break;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & botData.BotDetectionStats.Edge.value) != 0)
            {
                StartCoroutine(MoveStartTimer());
            }
        }

        private IEnumerator MoveStartTimer()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            botData.BotDetectionStats.IsOnEdge = false;

        }
    }
}