using System;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public CardInfo cardInfo;
    CardsDeck cardsDeck;
    
    Button button;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] TextMeshProUGUI stockText;

    public int stock;

    public UnityEvent onLeftClick;
    public UnityEvent onRightClick;
    public UnityEvent onInit = new();
    public void RegisterBehaviour(UnityEngine.Events.UnityAction _beh) {
        onLeftClick.AddListener(_beh);
    }

    void Update()
    {
        stockText.text = stock.ToString();
    }

    public void Init()
    {
        button = GetComponent<Button>();
        cardsDeck = FindAnyObjectByType<CardsDeck>();

        label.text = cardInfo.cardName;

    }

    public void MoveCard() {
            if(_inBook) {
                _inBook = !cardsDeck.MoveCardToHand(this);
            }else {
                _inBook = cardsDeck.MoveCardToBook(this);
            }
    }

    public void InspectCard() {
        cardsDeck.InspectCard(this);
    }

    public void RemoveCardFromDeck() {
        cardsDeck.RemoveCard(this);
    }

    bool _inBook = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left) {
            onLeftClick.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {
            // _inBook = !_inBook;
            onRightClick.Invoke();
        }
    }
}
