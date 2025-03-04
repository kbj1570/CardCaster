public class Wild : CardData
{
    public Wild()
    {
        cardNum = 100;
        serventNum = 7;
        cardName = "야생동물";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Enemy;
        force = 2;
        cardGuideDescription = "";
        voidWalker = true;
        cardAbility = "";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Earth;
    }
}