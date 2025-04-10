using DG.Tweening;
using TMPro;
using UnityEngine;

public class MessageLabel : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();

        DOTween.To(() => textMeshProUGUI.alpha, x => textMeshProUGUI.alpha = x, 0, 5);
        Destroy(gameObject, 5);
    }
}
