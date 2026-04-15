using System.Collections;

public class RedSlime : ServantCardData
{
    public RedSlime()
    {
        cardNum = "103";
        cardName = "레드 슬라임";
        cardCost = 0;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "빨간색 액체로 구성되어 있으며, 흐물거리는 몸을 이용해 움직인다. \r\n주로 용암지대에서 자주 발견된다. \r\n뜨거운 환경에 익숙해서인지 온화한 성격을 지닌다. 그 표정은 실로 황홀한 표정을 보인다고 알려져 있다.";
        cardDesc = "";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Fire;
    }
}