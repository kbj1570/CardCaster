using System.Collections.Generic;

public class VioletLichLord : CardData
{
    public VioletLichLord()
    {
        cardNum = 4;
        cardName = "바이올렛 리치로드";
        cardCost = 2;
        cardType = ECardType.Servent;
        serventAttribute = EServentAttribute.Darkness;
        force = 1;
        cardGuideDescription = "";
        cardAbility = "소환시 묘지에서 원하는 마법카드를 1장 가져온다";
        cardTargetType = ECardTargetType.Select;
        
        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.TrashCountOver;
        preRequisite.count = 1;
        preRequisite.cardType = ECardType.Spell;
        preRequisites.Add(preRequisite);
    }
}