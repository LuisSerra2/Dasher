using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy _Instance;
    public static DontDestroy Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<DontDestroy>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }
}
