using System;
using UnityEngine;

public sealed class Updater : MonoBehaviour
{
    public event Action OnUpdate;

    private static Updater _instance;

    public static void Subscribe(Action action)
    {
        if (Application.isEditor)
            return;

        Instance.OnUpdate += action;
    }

    public static void Unsubscribe(Action action)
    {
        if (Application.isEditor)
            return;

        Instance.OnUpdate -= action;
    }

    private static Updater Instance
    {
        get
        {
            if (Application.isEditor)
                return null;

            if (_instance == null)
            {
                _instance = FindObjectOfType<Updater>();

                if (_instance == null)
                {
                    var singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<Updater>();
                    singletonObject.name = typeof(Updater) + " (Singleton)";

                    if (!Application.isEditor)
                        DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    private void OnApplicationQuit()
    {
        OnUpdate = null;
        _instance = null;
        Destroy(this);
    }

    private void OnDestroy()
    {
        OnUpdate = null;
        _instance = null;
        Destroy(this);
    }
}