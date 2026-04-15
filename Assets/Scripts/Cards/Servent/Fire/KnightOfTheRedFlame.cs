using System.Collections;
using UnityEngine;

public class KnightOfTheRedFlame : ServantCardData
{
    public KnightOfTheRedFlame()
    {
        cardNum = "117";
        cardName = "홍염의 기사";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 2;
        cardStoryDesc = "";
        cardDesc = "소멸될 시, 덱에서\r\n[창해의 기사]를 가져온다.";
		serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Fire;
    }


	public override IEnumerator DeathEffectExecute(BattleManager bm)
	{
		bm.SearchCardInDeck(new KnightOfTheAzure());
		yield return null;
	}
}