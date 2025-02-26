public class Wolf : CardData
{
    public Wolf()
    {
        cardNum = 100;
        serventNum = 7;
        cardName = "심연의 탐구자";
        cardCost = 2;
        cardType = ECardType.Servent;
        serventType = EServentType.Enemy;
        force = 3;
        cardGuideDescription = "";
        voidWalker = true;
        cardAbility = "이 소환수는 다른 소환수나 마법의 효과를 받지 않는다";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Dark;
    }
}