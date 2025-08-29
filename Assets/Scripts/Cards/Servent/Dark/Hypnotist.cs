using System.Collections;
using UnityEngine;

public class Hypnotist : ServentCardData
{
    public Hypnotist()
    {
        cardNum = "125";
        cardName = "최면술사";
        cardCost = 3;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 3;
        cardStoryDesc = "";
        cardDesc = "소환 시 다른 소환수를 혼란 상태로 한다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Dark;
    }

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}