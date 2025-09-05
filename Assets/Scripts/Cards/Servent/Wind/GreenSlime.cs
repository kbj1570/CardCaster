using System.Collections;
using UnityEngine;
public class GreenSlime : ServentCardData
{
    public GreenSlime()
    {
        cardNum = "104";
        cardName = "그린 슬라임";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
        cardDesc = "바람 속성 소환수가\r\n소환될 시, 포스를 1 얻는다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Wind;
    }

	public override IEnumerator NotifySummonEffectExecute(BattleManager bm, Servent servent)
	{
        if(servent.GetAttribute() == EServentAttribute.Wind && servent != bm.activatingServent)
        {
            bm.activatingServent.GainForce(1);
        }
		yield return null;
	}
}
