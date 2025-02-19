public class FlameLizard : CardData
{
    public FlameLizard()
    {
        cardNum = 10;
        serventNum = 3;
        cardName = "플레임리자드";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardGuideDescription = "";
        cardAbility = "";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Fire;
    }
}