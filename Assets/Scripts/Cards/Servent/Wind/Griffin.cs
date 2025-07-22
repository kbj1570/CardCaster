using System.Collections;

public class Griffin : ServentCardData
{
    public Griffin()
    {
        cardNum = "118";
        cardName = "그리핀";
        cardCost = 2;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 4;
        cardStoryDesc = "오래된 기록 속에는 독수리의 머리와 날개 사자의 몸통,독수리의 앞발, 사자의 뒷발을 가졌다고 전해진다. \r\n성격은 온화하고 조용히 지내며, 자신이 인정한 존재에게 충성한다.";
        cardDesc = "소환 시 자신의 바람 속성 소환수 수만큼 드로우한다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Wind;
    }

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override IEnumerator AttackEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override IEnumerator DefendEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override bool IsAbilityUsable(BattleManager bm)
	{
		return true;
	}
}
