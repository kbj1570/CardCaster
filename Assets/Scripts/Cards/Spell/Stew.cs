public class Stew : CardData
{
    public Stew()
    {
        cardNum = 7;
        spellNum = 6;
        cardName = "스튜";
        cardCost = 0;
        cardType = ECardType.Spell;
        cardGuideDescription = "";
        cardAbility = "자신의 HP를 1 회복한다.";
        cardTargetType = ECardTargetType.Selected;

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.None;

        preRequisites.Add(preRequisite);
    }

}