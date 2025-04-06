using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BoardActor : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] bool isUi;

    [SerializeField] BoardButton[] boardButtons;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right && isUi) CreateBoard();
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()){
            CreateBoard();
            print("A");
        }
    }

    void CreateBoard() {
        BoardAction.instance.CreateBoard(boardButtons);
    }
}

[Serializable]
public class BoardButton {
    public string label;
    public UnityEvent buttonAction;
}