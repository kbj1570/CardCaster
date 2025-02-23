using System.Collections.Generic;

public class CardData
{
    protected int cardNum;
    protected int serventNum;
    protected int spellNum;
    protected string cardName;
    protected string cardAbility;
    protected string cardGuideDescription;
    protected int force;
    protected int cardCost;
    protected ECardType cardType;
    protected ECardTargetType cardTargetType;
    protected EServentAttribute serventAttribute;
    protected EServentSize serventSize;
    protected EServentType serventType;
    protected List<PreRequisite> preRequisites;
    protected bool penetrate;
    protected bool voidWalker;

    public int GetCardNum(){return cardNum;}
    public int GetServentNum(){return serventNum;}
    public int GetSpellNum(){return spellNum;}
    public string GetCardName(){return cardName;}
    public string GetCardAbility(){return cardAbility;}
    public int GetForce(){return force;}
    public int GetCardCost(){return cardCost;}
    public EServentAttribute GetAttribute(){return serventAttribute;}
    public EServentSize GetSize(){return serventSize;}
    public ECardType GetCardType(){return cardType;}
    public ECardTargetType GetCardTargetType(){return cardTargetType;}
    public List<PreRequisite> GetPreRequisites(){return preRequisites;}
    public bool GetPenetrate(){return penetrate;}
    public bool GetVoidWalker(){return voidWalker;}
    public EServentType GetServentType(){return serventType;}

}
public struct PreRequisite
{
    public EPreRequisite preRequisite;
    public EServentAttribute serventAttribute;
    public ECardType cardType;
    public int count;
    public int cardNum;
}