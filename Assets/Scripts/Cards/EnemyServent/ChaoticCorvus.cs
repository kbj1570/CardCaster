public class ChaoticCorvus : CardData
{
    public ChaoticCorvus()
    {
        cardNum = 0;
        serventNum = 0;
        cardName = "까마귀";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Enemy;
        force = 3;
        cardGuideDescription = "";
        voidWalker = true;
        cardAbility = "공격을 받으면 소멸한다";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Dark;
    }
}