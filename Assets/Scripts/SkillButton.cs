using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string title;
    [SerializeField, TextArea] string description;

    TextMeshProUGUI _description;
    private void Start()
    {
        _description = GameObject.FindGameObjectWithTag("Description").GetComponent<TextMeshProUGUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _description.text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _description.text = "";
    }
}
