using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class CardsDeck : MonoBehaviour
{

    List<Card> hand = new();
    List<Card> book = new();

    [SerializeField] List<RectTransform> slots = new();

    [SerializeField] RectTransform handBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void PickupCardInHand(Card _card) {
        foreach(var hc in hand) {
            if(hc.cardInfo.cardName == _card.cardInfo.cardName) {
                hc.stock += _card.stock;
                return;
            }
        }
        var c = Instantiate(_card.gameObject, handBox.position, _card.transform.rotation, handBox).GetComponent<Card>();
        hand.Add(c);
    }

    public void PickupCardInBook(Card _card) {

    }

    public void MoveCardToBook(Card _card) {
        for(int i = 0; i < slots.Count; i++) {
            if( _card.cardInfo.cardSlot == i && slots[i].childCount == 0) {
                _card.transform.SetParent(slots[i].transform);
                hand.Remove(_card);
                book.Add(_card);
                break;
            }
        }
    }
    public void MoveCardToHand(Card _card) {
        foreach(var hc in hand) {
            if(hc.cardInfo.cardName == _card.cardInfo.cardName) {
                hc.stock += _card.stock;
                Destroy(_card.gameObject);
                return;
            }
        }

        _card.transform.SetParent(handBox.transform);
        book.Remove(_card);
        hand.Add(_card);
    }
}
