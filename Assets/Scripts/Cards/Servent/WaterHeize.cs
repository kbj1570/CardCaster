public class WaterHeize : CardData
{
    public WaterHeize()
    {
        cardNum = 13;
        serventNum = 5;
        cardName = "물의 정령 헤이즈";
        cardCost = 2;
        cardType = ECardType.Servent;
        force = 1;
        cardGuideDescription = "장난치는 것을 좋아하는 푸른 색의 정령. 불을 좋아하는 친구와 항상 같이 붙어다닌다.";
        cardAbility = "소환시 덱에서 [불의 정령 크림슨]를 1장 가져온다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Water;
    }
}