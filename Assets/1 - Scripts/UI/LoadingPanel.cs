using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private Image loadingIcon;

    private RectTransform iconRectTransform => loadingIcon.rectTransform;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        iconRectTransform.rotation *= Quaternion.Euler(0f, 0f, -1f);
    }
}
