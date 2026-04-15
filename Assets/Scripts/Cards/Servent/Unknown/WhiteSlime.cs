using System.Collections;

public class WhiteSlime : ServantCardData
{
    public WhiteSlime()
    {
        cardNum = "114";
        cardName = "화이트 슬라임";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "흐물거리는 몸을 이용해 움직인다.";
        cardDesc = "";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Light;
    }
}
