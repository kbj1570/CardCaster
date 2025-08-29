using System.Collections;

public class BlackSlime : ServentCardData
{
    public BlackSlime()
    {
        cardNum = "115";
        cardName = "블랙 슬라임";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "하얀색 액체로 구성되어 있으며, 흐물거리는 몸을 이용해 움직인다. \r\n 전설 속에 존재하는 천공 섬이라는 곳에서 지상으로 내려왔다고 하지만 실제로 어떤지는 아무도 모른다.";
        cardDesc = "";
        penetrate = true;
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Dark;
    }

}
