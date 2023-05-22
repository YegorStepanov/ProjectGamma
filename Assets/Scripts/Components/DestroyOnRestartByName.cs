using UnityEngine;

public sealed class DestroyOnRestartByName : MonoBehaviour
{
    private bool _isCreated;

    // Mirror loads the bootstrap scene on each client disconnect, so remove the old Managers to preserve component links
    private void Awake()
    {
        foreach (var duplicateComponent in FindObjectsOfType<DestroyOnRestartByName>(true))
        {
            // TODO
            // update to the latest version of Mirror and try to remove this component.
            // IMO it's a Mirror bug.
            if (duplicateComponent.name != name)
                continue;

            if (duplicateComponent._isCreated)
            {
                Debug.Log($"Destroying {name}");
                Destroy(duplicateComponent.gameObject);
            }
        }

        _isCreated = true;
    }
}