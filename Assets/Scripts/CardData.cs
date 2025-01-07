using System.Collections.Generic;

public class CardData
{
    public int cardNum;
    public int serventNum;
    public int spellNum;
    public string cardName;
    public string cardAbility;
    public string cardGuideDescription;
    public int force;
    public int cardCost;
    public ECardType cardType;
    public ECardTargetType cardTargetType;
    public EServentAttribute serventAttribute;
    public EServentSize serventSize;
    public List<PreRequisite> preRequisites;

    public int GetCardNum(){return cardNum;}
    public int GetServentNum(){return serventNum;}
    public int GetSpellNum(){return serventNum;}
    public string GetCardName(){return cardName;}
    public string GetCardAbility(){return cardAbility;}
    public int GetForce(){return force;}
    public int GetCardCost(){return cardCost;}
    public EServentAttribute GetAttribute(){return serventAttribute;}
    public EServentSize GetSize(){return serventSize;}
    public ECardType GetCardType(){return cardType;}
    public ECardTargetType GetCardTargetType(){return cardTargetType;}
    public List<PreRequisite> GetPreRequisites(){return preRequisites;}


}
public struct PreRequisite
{
    public EPreRequisite preRequisite;
    public EServentAttribute serventAttribute;
    public ECardType cardType;
    public int count;
    public string name;
}