using System.Collections.Generic;

public class BannedOne : CardData
{
    public BannedOne()
    {
        cardNum = 2;
        cardName = "금지된 자";
        cardCost = 99;
        spellNum = -1;
        cardType = ECardType.Spell;
        cardGuideDescription = "";
        cardAbility = "자신의 소환수를 모두 소멸시키고 그 수만큼 드로우한다";
        cardTargetType = ECardTargetType.Select;
    }

}