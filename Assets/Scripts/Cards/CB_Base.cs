using UnityEngine;

public class CB_Base : MonoBehaviour
{
    [SerializeField] CardInfo cardInfo;
    Card card;
    
    
    public virtual void Start()
    {
        card = GetComponent<Card>();
        // card.onInit.AddListener(Init);
        card.Init(cardInfo);
        card.RegisterBehaviour(Act);
    }
    

    // Update is called once per frame
    public virtual void Act()
    {
        card.stock--;
        if(card.stock <= 0) Destroy(gameObject);
    }
}
