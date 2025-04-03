using System;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public CardInfo cardInfo;
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

    public void Init(CardInfo _info)
    {
        button = GetComponent<Button>();
        cardsDeck = FindAnyObjectByType<CardsDeck>();

        cardInfo = _info;
        
        label.text = _info.cardName;

        onRightClick.AddListener(() => {
            if(_inBook) {
                cardsDeck.MoveCardToHand(this);
            }else {
                cardsDeck.MoveCardToBook(this);
            }
        });
    }
    bool _inBook = true;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left) {
            onLeftClick.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {
            _inBook = !_inBook;
            onRightClick.Invoke();
        }
    }
}
