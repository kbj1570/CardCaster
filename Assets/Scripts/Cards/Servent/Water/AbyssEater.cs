using System.Collections;
using UnityEngine;

public class AbyssEater : ServentCardData
{
    public AbyssEater()
    {
        cardNum = "112";
        cardName = "심연의 포식자";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
        cardDesc = "물 속성 소환수가 소멸될 시, 포스를 2 얻는다.";
		serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Water;
	}

	public override IEnumerator NotifyDeathEffectExecute(BattleManager bm, Servent servent)
	{
		if (servent.GetAttribute() == EServentAttribute.Water && servent != bm.activatingServent)
		{
			bm.activatingServent.GainForce(2);
		}
		yield return null;
	}
}
