using System.Collections;

public class GiantLarva : ServentCardData
{
    public GiantLarva()
    {
        cardNum = "128";
        cardName = "거대한 애벌레";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
        cardDesc = "";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Earth;
    }
}