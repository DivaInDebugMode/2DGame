using UnityEngine;

public class PlatformParent  : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.parent = transform;
    }

    private void OnCollisionExit(Collision other)
    {
        other.transform.parent = null;
    }
}