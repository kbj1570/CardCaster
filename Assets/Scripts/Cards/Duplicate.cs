using System.Collections.Generic;

public class Duplicate : CardData
{
    public Duplicate()
    {
        cardNum = 2;
        cardName = "듀플리케이트";
        cardCost = 1;
        cardType = ECardType.Spell;
        cardGuideDescription = "";
        cardAbility = "자신의 소환수 하나를 선택하고 2장 복사해서 덱에 넣는다";
        cardTargetType = ECardTargetType.Select;

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.SelectedServent;
        preRequisite.serventAttribute = EServentAttribute.None;

        preRequisites.Add(preRequisite);
    }

}