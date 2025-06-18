using System.Collections;

public abstract class SpellCardData : BattleCardData, ISpellCardEffect
{
	public abstract IEnumerator SpellEffectExecute(BattleManager bm);
	public abstract bool IsSpellUsable(BattleManager bm);
}