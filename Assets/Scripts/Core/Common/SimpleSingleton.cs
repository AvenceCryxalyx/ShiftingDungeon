using UnityEngine;
using System.Collections.Generic;

public class SimpleSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("A instance already exists");
            Destroy(this); //Or GameObject as appropriate
            return;
        }
        instance = this as T;
        DontDestroyOnLoad(this);
    }
}
