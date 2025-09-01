using System.Collections;

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
        cardDesc = "";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Wind;
    }
}
