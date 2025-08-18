public class Wild : EnemyServentCardData
{
    public Wild()
    {
        cardName = "야생동물";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Enemy;
        force = 1;
        cardDesc = "";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Earth;
    }
}