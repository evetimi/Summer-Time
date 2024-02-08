using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T: Component
{
    #region Innstance
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this as T;
    }
    #endregion
}

public class UISingleton<T> : MonoBehaviourSingleton<T> where T : Component {
    public bool IsEnabled { get; private set; }

    public virtual void Open() {
        IsEnabled = true;
    }

    public virtual void Close() {
        IsEnabled = false;
    }
}

public class MonoBehaviourService<T> : MonoBehaviour where T : Component
{
    #region Innstance
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this as T;
        DontDestroyOnLoad(this);
    }
    #endregion
}

public class UIService<T> : MonoBehaviourService<T> where T : Component {
    public bool IsEnabled { get; private set; }

    public virtual void Open() {
        IsEnabled = true;
    }

    public virtual void Close() {
        IsEnabled = false;
    }
}