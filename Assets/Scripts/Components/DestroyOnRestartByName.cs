using UnityEngine;

public sealed class DestroyOnRestartByName : MonoBehaviour
{
    private bool _isCreated;

    // Mirror loads the bootstrap scene on each client disconnect, so remove the old Managers to preserve component links
    private void Awake()
    {
        foreach (var duplicateComponent in FindObjectsOfType<DestroyOnRestartByName>(true))
        {
            if (duplicateComponent.name != name) // todo
                continue;

            if (duplicateComponent._isCreated)
            {
                Debug.Log($"Destroying {name}"); // todo
                Destroy(duplicateComponent.gameObject);
            }
        }

        _isCreated = true;
    }
}