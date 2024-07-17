using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component {
    protected static T instance;

    public static bool HasInstance => instance != null;
    public static T TryGetInstace() => HasInstance ? instance : null;

    public static T Instance {
        get {
            if (instance == null) {
                instance = FindAnyObjectByType<T>();
                if (instance == null) {
                    var go = new GameObject(typeof(T).Name + "Auto-Generated");
                    instance = go.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake() {
        InitializeSingleton();
    }

    protected virtual void InitializeSingleton() {
        if (!Application.isPlaying) return;

        instance = this as T;
    }

}
public class PersistentSingleton<T> : MonoBehaviour where T : Component {
    public bool AutoUnparentOnAwake = true;

    protected static T instance;

    public static bool HasInstance => instance != null;
    public static T TryGetInstace() => HasInstance ? instance : null;

    public static T Instance {
        get {
            if (instance == null) {
                instance = FindAnyObjectByType<T>();
                if (instance == null) {
                    var go = new GameObject(typeof(T).Name + "Auto-Generated");
                    instance = go.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake() {
        InitializeSingleton();
    }

    protected virtual void InitializeSingleton() {
        if (!Application.isPlaying) return;

        if (AutoUnparentOnAwake) {
            transform.SetParent(null);
        }

        if (instance == null) {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        } else {
            if (instance != null) {
                Destroy(gameObject);
            }
        }

    }
}

public class RegulatorSingleton<T> : MonoBehaviour where T : Component {
    protected static T instance;

    public static bool HasInstance => instance != null;

    public float InitializationTime { get; private set; }

    public static T Instance {
        get {
            if (instance == null) {
                instance = FindAnyObjectByType<T>();
                if (instance == null) {
                    var go = new GameObject(typeof(T).Name + "Auto-Generated");
                    go.hideFlags = HideFlags.HideAndDontSave;
                    instance = go.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake() {
        InitializeSingleton();
    }

    protected virtual void InitializeSingleton() {
        if (!Application.isPlaying) return;
        InitializationTime = Time.time;
        DontDestroyOnLoad(gameObject);

        T[] oldInstance = FindObjectsByType<T>(FindObjectsSortMode.None);
        foreach (T old in oldInstance) {
            if (old.GetComponent<RegulatorSingleton<T>>().InitializationTime < InitializationTime) {
                Destroy(old.gameObject);
            }
        }

        if (instance == null) {
            instance = this as T;
        } 

    }
}
