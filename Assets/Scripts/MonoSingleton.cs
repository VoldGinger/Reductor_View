using System;
using Unity.VisualScripting;
using UnityEngine;
public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            _instance = FindObjectOfType<T>();
            if (_instance != null)
                return _instance;

            _instance = new GameObject().AddComponent<T>();
            return _instance;

        }
    }
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            if (this != _instance)
            {
                Destroy(gameObject);
            }
        }
    }




}
