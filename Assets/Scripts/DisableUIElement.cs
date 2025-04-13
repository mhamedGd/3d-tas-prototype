using UnityEngine;
using UnityEngine.EventSystems;

public class DisableUIElement : MonoBehaviour, IPointerClickHandler
{
    CanvasGroup canvasGroup;

    public void OnPointerClick(PointerEventData eventData)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
