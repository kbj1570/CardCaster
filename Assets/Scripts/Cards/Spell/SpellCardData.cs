using System.Collections;

public abstract class SpellCardData : CardData, ICardEffect
{
	public virtual IEnumerator SummonEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator AttackEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator DefendEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator DeathEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator HitEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator ActivationEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator StandByPhaseEffectExecute(BattleManager bm) { yield break; }
	public virtual IEnumerator EndPhaseEffectExecute(BattleManager bm) { yield break; }
}