using System.Collections;

public class BlackSlime : ServentCardData
{
    public BlackSlime()
    {
        cardNum = "111";
        cardName = "블랙 슬라임";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
        cardDesc = "어둠 속성 소환수가\r\n소환될 시, 그 소환수의\r\n포스를 흡수한다.";
		penetrate = true;
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Dark;
    }
	public override IEnumerator NotifySummonEffectExecute(BattleManager bm, Servent servent)
	{
		if (servent.GetAttribute() == EServentAttribute.Dark && servent != bm.activatingServent)
		{
            int force = servent.GetForce();
			servent.SetForce(0);
            bm.activatingServent.GainForce(force);
		}
		yield return null;
	}
}
