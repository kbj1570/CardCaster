using System.Collections;

public abstract class SpellCardData : CardData, ICardEffect
{
	public abstract IEnumerator SummonEffectExecute(BattleManager bm);
	public abstract IEnumerator AttackEffectExecute(BattleManager bm);
	public abstract IEnumerator DefendEffectExecute(BattleManager bm);
	public abstract IEnumerator DeathEffectExecute(BattleManager bm);
	public abstract IEnumerator HitEffectExecute(BattleManager bm);
	public abstract IEnumerator ActivationEffectExecute(BattleManager bm);

	public abstract IEnumerator StandByPhaseEffectExecute(BattleManager bm);
	public abstract IEnumerator EndPhaseEffectExecute(BattleManager bm);
}