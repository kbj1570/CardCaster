public class OnlySilence : CardData
{
    public OnlySilence()
    {
        cardNum = 6;
        spellNum = 5;
        cardName = "오직 침묵만이";
        cardCost = 0;
        cardType = ECardType.Spell;
        cardGuideDescription = "";
        cardAbility = "자신의 HP가 1일때만 사용할 수 있다. 서로의 소횐수들을 전부 소멸시킨다";
        cardTargetType = ECardTargetType.Selected;

        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.PlayerHPCount;
        preRequisite.count = 1;

        preRequisites.Add(preRequisite);


        preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.AllServentCountOver;
        preRequisite.count = 0;

        preRequisite.serventAttribute = EServentAttribute.None;

        preRequisites.Add(preRequisite);

    }

}