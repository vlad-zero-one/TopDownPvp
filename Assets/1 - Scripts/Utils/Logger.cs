using DependencyInjection;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public void Init()
    {
        DontDestroyOnLoad(this);

        DI.Add(this);
    }

    public void Log<T>(T message)
    {
        Debug.Log(message.ToString());
    }
}