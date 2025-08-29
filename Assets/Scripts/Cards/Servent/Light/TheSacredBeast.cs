using System.Collections;
using System.Collections.Generic;

public class TheSacredBeast : ServentCardData
{
	public TheSacredBeast()
	{
		cardNum = "121";
		cardName = "숲의 신수 백록";
		cardCost = 1;
		cardType = ECardType.Servent;
		serventType = EServentType.Player;
		force = 3;
		cardStoryDesc = "";
		cardDesc = "소환 시 자신의 Hp가 5 이하라면 상대 소환수를 무작위로 소멸시킨다.";
		serventSize = EServentSize.Small;
		cardTargetType = ECardTargetType.Select;
		serventAttribute = EServentAttribute.Light;
	}

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}
