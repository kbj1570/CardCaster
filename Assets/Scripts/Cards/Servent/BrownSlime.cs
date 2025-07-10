using System.Collections;

public class BrownSlime : ServentCardData
{
    public BrownSlime()
    {
        cardNum = "113";
        cardName = "브라운 슬라임";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "갈색 액체로 구성되어 있으며, 흐물거리는 몸을 이용해 움직인다. 주로 가파른 산 정상에서 자주 발견된다.\r\n슬라임들 중에서 움직임 거의 없는 특이한 개체라고 알려져 있다. ";
        cardDesc = "";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Earth;
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