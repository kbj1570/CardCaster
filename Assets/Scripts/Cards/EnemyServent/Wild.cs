public class Wild : EnemyServentCardData
{
    public Wild()
    {
        cardName = "야생동물";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Enemy;
        force = 2;
        cardStoryDesc = "";
        cardAbility = "";
        canUseAbility = true;
        hasAbility = true;
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Earth;
    }
}