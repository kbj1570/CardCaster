public class MaskedWorld : CardData
{
    public MaskedWorld()
    {
        cardNum = 15;
        cardName = "마스크월드";
        cardCost = 0;
        spellNum = 9;
        cardType = ECardType.Spell;
        cardGuideDescription = ""; 
        cardAbility = "모든 소환수들의 포스를 1 증가시킨다.";
        cardTargetType = ECardTargetType.Selected;

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.AllServentCountOver;
        preRequisite.count = 0;
        preRequisite.cardType = ECardType.None;

        preRequisites.Add(preRequisite);
    }

}