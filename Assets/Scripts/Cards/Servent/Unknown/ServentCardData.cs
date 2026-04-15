using System.Collections;

public abstract class ServantCardData : CardData
{
	protected EServentAttribute serventAttribute;
	protected EServentSize serventSize;
	protected EServentType serventType;

	protected int force;
	protected bool penetrate;
	protected bool hasActivationEffect;

	protected bool voidWalker;
	protected bool fireImmune;
	protected bool waterImmune;
	protected bool windImmune;
	protected bool darkImmune;
	protected bool lightImmune;

	public int GetForce() => force;
	public EServentType GetServentType() => serventType;
	public EServentAttribute GetAttribute() => serventAttribute;
	public EServentSize GetSize() => serventSize;
	public bool GetPenetrate() => penetrate;
	public bool GetHasActivtionEffect() => hasActivationEffect;
	public bool GetVoidWalker() => voidWalker;
}