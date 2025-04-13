using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] GameObject messagePrefab;
    [SerializeField] RectTransform container;
    public static Message instance;

    void Awake()
    {
        if(instance == null) instance = this;
        if(instance != this) Destroy(this);
    }

    public void Send(string _content) {
        Instantiate(messagePrefab, Vector3.zero, messagePrefab.transform.rotation, container).GetComponent<TextMeshProUGUI>().text = _content;
    }
}
