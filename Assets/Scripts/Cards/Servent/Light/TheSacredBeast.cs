using System.Collections;
using System.Collections.Generic;

public class TheSacredBeast : ServantCardData
{
	public TheSacredBeast()
	{
		cardNum = "121";
		cardName = "���� �ż� ���";
		cardCost = 1;
		cardType = ECardType.Servent;
		serventType = EServentType.Player;
		force = 3;
		cardStoryDesc = "";
		cardDesc = "��ȯ �� �ڽ��� Hp�� 5 ���϶�� ��� ��ȯ���� �������� �Ҹ��Ų��.";
		serventSize = EServentSize.Small;
		cardTargetType = ECardTargetType.Targeting;
		serventAttribute = EServentAttribute.Light;
	}

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}
