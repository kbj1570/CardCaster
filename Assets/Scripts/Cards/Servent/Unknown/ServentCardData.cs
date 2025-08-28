using System.Collections;

public abstract class ServentCardData : CardData, ICardEffect
{
	protected EServentAttribute serventAttribute;
	protected EServentSize serventSize;
	protected EServentType serventType;
	protected EAbilityType abilityType;

	protected int force;


	protected int strength;
	protected int intelligence;
	protected int height;
	protected int weight;
	protected bool penetrate;
	protected bool voidWalker;
	protected bool fireImmune;
	protected bool waterImmune;
	protected bool windImmune;
	protected bool darkImmune;
	protected bool lightImmune;

	public int GetForce() { return force; }
	public EServentType GetServentType() { return serventType; }
	public EServentAttribute GetAttribute() { return serventAttribute; }
	public EAbilityType GetAbilityType() { return abilityType; }
	public EServentSize GetSize() { return serventSize; }
	public bool GetPenetrate() { return penetrate; }
	public bool GetVoidWalker() { return voidWalker; }
	public abstract IEnumerator SummonEffectExecute(BattleManager bm);
	public abstract IEnumerator AttackEffectExecute(BattleManager bm);
	public abstract IEnumerator DefendEffectExecute(BattleManager bm);
	public abstract IEnumerator DeathEffectExecute(BattleManager bm);
	public abstract IEnumerator HitEffectExecute(BattleManager bm);
	public abstract IEnumerator ActivationEffectExecute(BattleManager bm);

	public abstract IEnumerator StandByPhaseEffectExecute(BattleManager bm);
	public abstract IEnumerator EndPhaseEffectExecute(BattleManager bm);
}