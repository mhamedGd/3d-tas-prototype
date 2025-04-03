using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "Scriptable Objects/CardInfo")]
public class CardInfo : ScriptableObject
{
    public string cardName;
    [TextArea]public string description;
    public int cardSlot;
}
