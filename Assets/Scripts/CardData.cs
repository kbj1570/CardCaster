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
    public EMonsterAttribute monsterAttribute;
    // public List<CardDataSO> requirements;

    public int GetCardNum(){return cardNum;}
    public string GetCardName(){return cardName;}
    public string GetCardAbility(){return cardAbility;}
    public int GetForce(){return force;}
    // public Sprite GetSprite(){return sprite;}
    // public int GetCardCount() {return cardCount;}
    public int GetCardCost(){return cardCost;}
    public EMonsterAttribute GetAttribute(){return monsterAttribute;}
    public ECardType GetCardType(){return cardType;}
}
