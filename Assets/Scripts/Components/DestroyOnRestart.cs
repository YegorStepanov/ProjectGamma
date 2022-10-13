using UnityEngine;

public sealed class DestroyOnRestart : MonoBehaviour
{
    private bool _created;

    // Mirror loads the bootstrap scene on each host disconnect so remove the old Managers to preserve component links
    private void Awake()
    {
        foreach (var singleton in FindObjectsOfType<DestroyOnRestart>(true))
        {
            if (singleton.name != name) //todo
                continue;

            if (singleton._created)
            {
                Debug.Log($"Destroying {name}");
                Destroy(singleton.gameObject);
            }
        }

        _created = true;
    }
}