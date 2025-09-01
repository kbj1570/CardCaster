using System.Collections;

public class ShapeShifter : ServentCardData
{
    public ShapeShifter()
    {
        cardNum = "112";
        cardName = "셰이프 시프터";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 3;
        cardStoryDesc = "";
        cardDesc = "";
		serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Earth;
    }

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}
