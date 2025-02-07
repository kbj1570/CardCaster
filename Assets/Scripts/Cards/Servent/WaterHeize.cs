public class WaterHeize : CardData
{
    public WaterHeize()
    {
        cardNum = 14;
        serventNum = 5;
        cardName = "물의 정령 헤이즈";
        cardCost = 0;
        cardType = ECardType.Servent;
        force = 1;
        cardGuideDescription = "장난치는 것을 좋아하는 푸른 색의 정령. 불을 좋아하는 친구와 항상 같이 붙어다닌다.";
        cardAbility = "소환시 덱에서 [불의 정령 크림슨]를 1장 가져온다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Water;



        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.DeckCountOver;
        preRequisite.count = 0;
        preRequisite.cardType = ECardType.None;
        preRequisite.cardNum = 13;

        preRequisites.Add(preRequisite);
    }
}