public class CardData
{
    public int cardNum;
    public string cardName;
    public string cardAbility;
    public string cardBookDescription;
    public int force;
    // public Sprite sprite;
    public int cardCost;
    public ECardType cardType;
    // public List<ActionData> cardEffect;
    public EServentAttribute serventAttribute;
    public EServentSize serventSize;
    // public List<CardDataSO> requirements;

    public int GetCardNum(){return cardNum;}
    public string GetCardName(){return cardName;}
    public string GetCardAbility(){return cardAbility;}
    public int GetForce(){return force;}
    // public Sprite GetSprite(){return sprite;}
    // public int GetCardCount() {return cardCount;}
    public int GetCardCost(){return cardCost;}
    public EServentAttribute GetAttribute(){return serventAttribute;}
    public EServentSize GetSize(){return serventSize;}
    public ECardType GetCardType(){return cardType;}
}
