using System.Collections;

public class Frillizard : ServentCardData
{
    public Frillizard()
    {
        cardNum = "117";
        cardName = "목도리 슬라임";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "주로 척박한 사막지대에서 서식하는 도마뱀 몬스터이다.\r\n항상 배가 고파 사막을 횡단하는 모험가들을 공격한다고 알려져 있다.\r\n먹이감을 포착하면 목에있는 프릴을 펼치면서 쫓아온다.";
        cardDesc = "소환 시 상대 소환수 1마리를 혼란 상태로 만든다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Dark;
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
