public class WillOfBerserker : CardData
{
    public WillOfBerserker()
    {
        cardNum = 17;
        cardName = "투사의 의지";
        cardCost = 0;
        spellNum = 10;
        cardType = ECardType.Spell;
        cardGuideDescription = ""; 
        cardAbility = "자신의 소환수 하나의 포스를 2배로 한다. 그 소환수는 이 턴이 끝나면 소멸한다.";
        cardTargetType = ECardTargetType.Select;

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.SelectedServent;
        preRequisite.serventAttribute = EServentAttribute.None;

        preRequisites.Add(preRequisite);
    }

}