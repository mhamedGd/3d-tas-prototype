using UnityEngine;

public class CardPickable : Interactable
{
    public override void Interact(Player p)
    {
        base.Interact(p);
        if(cardsDeck.PickupCardInHand(correspondingCard) != null) Destroy(gameObject);
    }
    CardsDeck cardsDeck;
    [SerializeField] Card correspondingCard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardsDeck = FindAnyObjectByType<CardsDeck>();
    }

}
