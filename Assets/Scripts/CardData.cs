public class CardData
{
    public int cardNum;
    public string cardName;
    public string cardAbility;
    public string cardGuideDescription;
    public int force;
    public int cardCost;
    public ECardType cardType;
    public ECardTargetType cardTargetType;
    public EServentAttribute serventAttribute;
    public EServentSize serventSize;

    public int GetCardNum(){return cardNum;}
    public string GetCardName(){return cardName;}
    public string GetCardAbility(){return cardAbility;}
    public int GetForce(){return force;}
    public int GetCardCost(){return cardCost;}
    public EServentAttribute GetAttribute(){return serventAttribute;}
    public EServentSize GetSize(){return serventSize;}
    public ECardType GetCardType(){return cardType;}
    public ECardTargetType GetCardTargetType(){return cardTargetType;}
}
