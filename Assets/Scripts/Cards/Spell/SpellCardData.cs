using System.Collections;

public abstract class SpellCardData : BattleCardData, ISpellCardEffect
{

	protected int spellNum;
	
	public int GetSpellNum() { return spellNum; }
	public abstract IEnumerator SpellEffectExecute(BattleManager bm);
	public abstract bool IsSpellUsable(BattleManager bm);
}