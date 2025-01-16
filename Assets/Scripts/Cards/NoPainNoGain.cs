
public class NoPainNoGain : CardData
{
    public NoPainNoGain()
    {
        cardNum = 5;
        spellNum = 4;
        cardName = "작은 것을 위한 희생";
        cardCost = 1;
        cardType = ECardType.Spell;
        cardGuideDescription = "";
        cardAbility = "자신의 묘지의 카드를 전부 덱으로 되돌린다. 그리고 그 수만큼 자신은 HP를 잃는다.";
        cardTargetType = ECardTargetType.Selected;

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.TrashCountOver;
        preRequisite.count = 1;

        preRequisites.Add(preRequisite);
    }

}