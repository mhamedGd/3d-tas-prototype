using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CardsDeck : MonoBehaviour
{

    List<Card> hand = new();
    List<Card> book = new();

    [SerializeField] List<RectTransform> slots = new();

    [SerializeField] RectTransform handBox;

    [Header("Inspection Related")]
    [SerializeField] CanvasGroup inspectionScreen;
    [SerializeField] TextMeshProUGUI inspectionLabel;
    [SerializeField] TextMeshProUGUI inspectionLabel2;
    [SerializeField] TextMeshProUGUI inspectionStock;
    [SerializeField] TextMeshProUGUI inspectionDescription;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void RemoveCard(Card _card) {
        foreach(var c in hand) {
            if(c == _card){
                hand.Remove(_card);
                return;
            }
        }

        foreach(var c in book) {
            if(c == _card){
                hand.Remove(_card);
                return;
            }
        }
    }
    public Card PickupCardInHand(Card _card) {
        Message.instance.Send($"Picked up {_card.cardInfo.cardName}");
        if(hand.Count == 2){
            return PickupCardInBook(_card);
        }
        foreach(var hc in hand) {
            if(hc.cardInfo == _card.cardInfo) {
                hc.stock += _card.stock;
                return hc;
            }
        }
        var c = Instantiate(_card.gameObject, handBox.position, _card.transform.rotation, handBox).GetComponent<Card>();
        hand.Add(c);
        return c;
    }

    public Card PickupCardInBook(Card _card) {
        for(int i = 0;i < slots.Count;i++) {
            if(_card.cardInfo.cardSlot == i) {
                if(slots[i].childCount != 0) {
                    // Slot Taken
                    break;
                }
                var c = Instantiate(_card.gameObject, handBox.position, _card.transform.rotation, slots[i]).GetComponent<Card>();
                book.Add(c);
                return c;
            }
        }

        return null;
    }

    public bool MoveCardToBook(Card _card) {
        for(int i = 0; i < slots.Count; i++) {
            if( _card.cardInfo.cardSlot == i && slots[i].childCount == 0) {
                _card.transform.SetParent(slots[i].transform);
                hand.Remove(_card);
                book.Add(_card);
                return true;
            }
        }

        return false;
    }
    public bool MoveCardToHand(Card _card) {
        foreach(var hc in hand) {
            if(hc.cardInfo.cardName == _card.cardInfo.cardName) {
                hc.stock += _card.stock;
                Destroy(_card.gameObject);
                return true;
            }
        }
        if(hand.Count == 2) return false;

        _card.transform.SetParent(handBox.transform);
        book.Remove(_card);
        hand.Add(_card);

        return true;
    }

    public void InspectCard(Card _card) {
        inspectionScreen.alpha = 1;
        inspectionScreen.interactable = true;
        inspectionScreen.blocksRaycasts = true;

        inspectionLabel.text = _card.cardInfo.cardName;
        inspectionLabel2.text = _card.cardInfo.cardName;
        inspectionStock.text = _card.stock.ToString();
        inspectionDescription.text = _card.cardInfo.description;
    }
    public void StopInspecting(){
        inspectionScreen.alpha = 0;
        inspectionScreen.interactable = false;
        inspectionScreen.blocksRaycasts = false;
    }
}
