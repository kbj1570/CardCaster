using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CardData : ICardEffect
{

	protected string cardNum;
	protected string cardName;
	protected string cardDesc;
	protected string cardStoryDesc;
	protected ECardType cardType;
	protected ESpellType spellType;
	protected ECardTargetType cardTargetType;
	protected ECardRarity cardRarity;
	protected int cardCost;

	public bool hasStatusEffect;

	public EStatusCondition[] statusConditions;

	protected int fontSize;
	public int GetCardCost() { return cardCost; }


	public string GetCardNum(){return cardNum;}
	public string GetCardName(){return cardName;}
	public string GetCardDesc(){return cardDesc;}

	public string GetCardStoryDesc() { return cardStoryDesc; }
	public ECardType GetCardType(){return cardType;}
	public ECardRarity GetCardRarity() { return cardRarity; }
	public ECardTargetType GetCardTargetType(){return cardTargetType;}

	public virtual IEnumerator SummonEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator AttackEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator DefendEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator DeathEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator HitEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator ActivationEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator StandByPhaseEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator EndPhaseEffectExecute(BattleManager bm) { yield break; }
	public virtual bool IsCardUsable(BattleManager bm) { return true; }
	public virtual IEnumerator NotifySummonEffectExecute(BattleManager bm, Servent servent) { yield break; }
	public virtual IEnumerator NotifyDeathEffectExecute(BattleManager bm, Servent servent) { yield break; }
}
public interface ICardEffect
{
	IEnumerator SummonEffectExecute(BattleManager bm);
	IEnumerator AttackEffectExecute(BattleManager bm);
	IEnumerator DefendEffectExecute(BattleManager bm);
	IEnumerator HitEffectExecute(BattleManager bm);
	IEnumerator DeathEffectExecute(BattleManager bm);
	IEnumerator StandByPhaseEffectExecute(BattleManager bm);
	IEnumerator EndPhaseEffectExecute(BattleManager bm);
	IEnumerator ActivationEffectExecute(BattleManager bm);
	IEnumerator NotifySummonEffectExecute(BattleManager bm, Servent servent);
	IEnumerator NotifyDeathEffectExecute(BattleManager bm, Servent servent);
}