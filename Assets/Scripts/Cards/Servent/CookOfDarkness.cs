public class CookOfDarkness : CardData
{
    public CookOfDarkness()
    {
        cardNum = 9;
        serventNum = 2;
        cardName = "암흑요리사";
        cardCost = 0;
        cardType = ECardType.Servent;
        force = 1;
        cardGuideDescription = "";
        cardAbility = "소환시 자신의 덱에서 [스튜]를 2장 가져온다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Dark;

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.DeckCountOver;
        preRequisite.count = 0;
        preRequisite.cardType = ECardType.None;
        preRequisite.cardNum = 7;

        preRequisites.Add(preRequisite);
    }
}