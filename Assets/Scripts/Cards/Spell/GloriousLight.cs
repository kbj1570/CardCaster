using System.Collections.Generic;

public class GloriousLight : CardData
{
    public GloriousLight()
    {
        cardNum = 3;
        spellNum = 2;
        cardName = "악을 멸하는 등불";
        cardCost = 1;
        cardType = ECardType.Spell;
        cardGuideDescription = "";
        cardAbility = "자신의 빛 속성 소환수가 있을 때 사용할 수 있다. 어둠 소환수들을 전부 소멸시킨다";
        cardTargetType = ECardTargetType.Selected;
        
        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.AllServentCountOver;
        preRequisite.count = 0;
        preRequisite.serventAttribute = EServentAttribute.Dark;
        preRequisites.Add(preRequisite);

        preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.PlayerServentCountOver;
        preRequisite.count = 0;                                                                 
        preRequisite.serventAttribute = EServentAttribute.Light;
        
        preRequisites.Add(preRequisite);
    }
}