using System.Collections;
using System.Collections.Generic;

public class TheSacredBeast : ServantCardData
{
	public TheSacredBeast()
	{
		cardNum = "121";
		cardName = "미지의 성수";
		cardCost = 1;
		cardType = ECardType.Servent;
		serventType = EServentType.Player;
		force = 3;
		cardStoryDesc = "";
		cardDesc = "HP가 5 이하일 경우에 사용할 수 있다. 상대의 소환수 하나를 소멸시킨다.";
		serventSize = EServentSize.Small;
		cardTargetType = ECardTargetType.Targeting;
		serventAttribute = EServentAttribute.Light;
	}

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}
