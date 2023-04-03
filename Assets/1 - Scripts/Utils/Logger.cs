using DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
    [SerializeField] private Text logText;
    [SerializeField] private Button clearButton;

    private void Awake()
    {
        if (DI.Has<Logger>())
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        clearButton.onClick.AddListener(Clear);

        DI.Add(this);
    }

    private void Clear()
    {
        if (this == null || gameObject == null || logText == null) return;

        logText.text = string.Empty;
    }

    public void Log<T>(T message)
    {
        if (this == null || gameObject == null || logText == null) return;

        Debug.Log(message.ToString());

        logText.text += message.ToString();
        logText.text += "\n";
    }

    private void OnDestroy()
    {
        clearButton.onClick.RemoveListener(Clear);
    }
}