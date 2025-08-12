public class CorvusShadow : EnemyServentCardData
{
    public CorvusShadow()
    {
        cardName = "까마귀";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Enemy;
        force = 3;
        hasAbility = true;
        cardStoryDesc = "";
        cardDesc = "공격을 받으면 소멸한다";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Dark;
    }
}