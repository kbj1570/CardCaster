using System.Collections;

public class ForbiddenedSavior : ServentCardData
{
    public ForbiddenedSavior()
    {
        cardNum = "119";
        cardName = "봉인된 수호자";
        cardCost = 2;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
		darkImmune = true;
		force = 4;
        cardStoryDesc = "먼 과거 잊혀진 왕국의 전설적인 기사.\r\n위대한 존재에게 선택 받아 성스러운 힘을 부여받았지만,\r\n그 힘은 인간이 감당하기엔 지나치게 순수하고 강력했다.\r\n그 힘이 자신을 타락시키고 있음을 깨달은 기사는\r\n스스로의 의지로 성스러운 힘의 일부를 봉인했다.";
        cardDesc = "어둠 속성 소환수로부터 대미지를 받지 않는다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Light;
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
		return false;
	}
}
