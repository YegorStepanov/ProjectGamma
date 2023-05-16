using UnityEngine;

public sealed class DontDestroyOnLoad : MonoBehaviour
{
    // We cannot set the flag in Awake because it moves a gameobject to another scene (Mirror has not initialized yet), so it throws an exception when the client tries to connect:
    // "Spawn scene object not found for [GUID]. Make sure that client and server use exactly the same project. This only happens if the hierarchy gets out of sync."
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
