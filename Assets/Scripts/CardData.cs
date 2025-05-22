using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CardData
{

	protected int cardNum;
	protected string cardName;
	protected string cardAbility;
	protected string cardGuideDescription;
	protected ECardType cardType;
	protected ECardTargetType cardTargetType;
	protected List<PreRequisite> preRequisites;



	public int GetCardNum(){return cardNum;}
	public string GetCardName(){return cardName;}
	public string GetCardAbility(){return cardAbility;}
	public ECardType GetCardType(){return cardType;}
	public ECardTargetType GetCardTargetType(){return cardTargetType;}
	public List<PreRequisite> GetPreRequisites(){return preRequisites;}
}

public class BattleCardData : CardData
{

	protected int cardCost;
	public int GetCardCost() { return cardCost; }
}
	public struct PreRequisite
{
	public EPreRequisite preRequisite;
	public EServentAttribute serventAttribute;
	public ECardType cardType;
	public int count;
	public int cardNum;
}

public interface IServentCardEffect
{
	public IEnumerator SummonEffectExecute(BattleManager bm);
	public IEnumerator AttackEffectExecute(BattleManager bm);
	public IEnumerator DefendEffectExecute(BattleManager bm);
}

public interface ISpellCardEffect
{
	public IEnumerator SpellEffectExecute(BattleManager bm);
}