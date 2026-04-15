using System.Collections;

public class KnightOfTheAzure : ServantCardData
{
    public KnightOfTheAzure()
    {
        cardNum = "118";
        cardName = "창해의 기사";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
		cardDesc = "소멸될 시, 덱에서\r\n[홍염의 기사]를 가져온다.";
		serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Water;
    }
	public override IEnumerator DeathEffectExecute(BattleManager bm)
	{
		bm.SearchCardInDeck(new KnightOfTheRedFlame());
		yield return null;
	}
}