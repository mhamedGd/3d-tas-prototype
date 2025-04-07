using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardUIButton : MonoBehaviour, IPointerClickHandler
{
    UnityEvent onClick = new();
    [SerializeField] TextMeshProUGUI label;
    Button b;
    public void InitButton(BoardButton _info, BoardAction _action) {
        label.text = _info.label;
        b=GetComponent<Button>();
        onClick = _info.buttonAction;
        onClick.AddListener(() => {
            _action.DestroyChoices();
        });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left) onClick.Invoke();
    }
}
