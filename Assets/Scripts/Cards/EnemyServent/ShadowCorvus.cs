public class ShadowCorvus : EnemyServentCardData
{
    public ShadowCorvus()
    {
        cardName = "그림자 까마귀";
		cardNum = "102";
		cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Enemy;
        force = 1;
        cardStoryDesc = "";
        cardDesc = "공격을 받을 시, 상대에게";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Dark;
   }
}