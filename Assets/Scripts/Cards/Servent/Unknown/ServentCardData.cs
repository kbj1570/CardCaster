using System.Collections;

public abstract class ServentCardData : BattleCardData, IServentCardEffect
{
	protected EServentAttribute serventAttribute;
	protected EServentSize serventSize;
	protected EServentType serventType;

	protected int force;

	protected int strength;
	protected int intelligence;
	protected int height;
	protected int weight;


	protected bool hasAbility;
	protected bool canUseAbility;
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
	public EServentSize GetSize() { return serventSize; }
	public bool GetPenetrate() { return penetrate; }
	public bool GetVoidWalker() { return voidWalker; }
	public bool GetHasAbility() { return hasAbility; }
	public bool GetCanUseAbility() { return canUseAbility; }
	public abstract IEnumerator SummonEffectExecute(BattleManager bm);
	public abstract IEnumerator AttackEffectExecute(BattleManager bm);
	public abstract IEnumerator DefendEffectExecute(BattleManager bm);
	public abstract bool IsAbilityUsable(BattleManager bm);
}