using UnityEngine;

public sealed class LookAtMainCamera : MonoBehaviour
{
    // LateUpdate so that all camera updates are finished.
    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
