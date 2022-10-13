using UnityEngine;

public sealed class DestroyOnRestart : MonoBehaviour
{
    private bool _created;

    // Mirror loads the bootstrap scene on each host disconnect so remove the old Managers to preserve component links
    private void Awake()
    {
        foreach (var duplicate in FindObjectsOfType<DestroyOnRestart>(true))
        {
            if (duplicate.name != name)
                continue;

            if (duplicate._created)
            {
                Debug.Log($"Destroying {name}");
                Destroy(duplicate.gameObject);
            }
        }

        _created = true;
    }
}