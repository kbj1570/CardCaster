using System.Collections;

public class BlueSlime : ServentCardData
{
    public BlueSlime()
    {
        cardNum = "102";
        cardName = "블루 슬라임";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "파란색 액체로 구성되어 있으며, 흐물거리는 몸을 이용해 움직인다. \r\n주로 강가나 시냇물이 있는 지역에서 자주 발견된다. 경계심이 적고 친화력이 뛰어나 다른 생명체들에게 인기가 많으며, \r\n어려움에처한 모험가나 여행자에게 도움을 주는 존재로 알려져 있다.";
        cardDesc = "";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Water;
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
	public override IEnumerator DeathEffectExecute(BattleManager bm)
	{
		yield return null;
	}

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}