using UnityEngine;

public class CB_Base : MonoBehaviour
{
    Card card;
    
    
    public virtual void Start()
    {
        card = GetComponent<Card>();
        // card.onInit.AddListener(Init);
        card.Init();
        card.RegisterBehaviour(Act);
    }
    

    // Update is called once per frame
    public virtual void Act()
    {
        card.stock--;
        if(card.stock <= 0){
            card.RemoveCardFromDeck();
            Destroy(gameObject);
        }
    }
}
