using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CardData
{

	protected string cardNum;
	protected string cardName;
	protected string cardDesc;
	protected string cardStoryDesc;
	protected ECardType cardType;
	protected ESpellType spellType;
	protected ECardTargetType cardTargetType;
	protected ECardRarity cardRarity;
	protected List<PreRequisite> preRequisites;
	protected int cardCost;

	protected int fontSize;
	public int GetCardCost() { return cardCost; }


	public string GetCardNum(){return cardNum;}
	public string GetCardName(){return cardName;}
	public string GetCardDesc(){return cardDesc;}

	public string GetCardStoryDesc() { return cardStoryDesc; }
	public ECardType GetCardType(){return cardType;}
	public ECardRarity GetCardRarity() { return cardRarity; }
	public ECardTargetType GetCardTargetType(){return cardTargetType;}
	public List<PreRequisite> GetPreRequisites(){return preRequisites;}
}

public struct PreRequisite
{
	public EPreRequisite preRequisite;
	public EServentAttribute serventAttribute;
	public ECardType cardType;
	public int count;
	public string cardNum;
}

public interface ICardEffect
{
	public IEnumerator SummonEffectExecute(BattleManager bm);
	public IEnumerator AttackEffectExecute(BattleManager bm);
	public IEnumerator DefendEffectExecute(BattleManager bm);
	public IEnumerator HitEffectExecute(BattleManager bm);
	public IEnumerator DeathEffectExecute(BattleManager bm);
	public IEnumerator StandByPhaseEffectExecute(BattleManager bm);
	public IEnumerator EndPhaseEffectExecute(BattleManager bm);
	public IEnumerator ActivationEffectExecute(BattleManager bm);
}