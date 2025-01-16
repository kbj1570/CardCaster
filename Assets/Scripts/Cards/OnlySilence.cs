public class OnlySilence : CardData
{
    public OnlySilence()
    {
        cardNum = 3;
        spellNum = 5;
        cardName = "침묵만이 남으리";
        cardCost = 0;
        cardType = ECardType.Spell;
        cardGuideDescription = "";
        cardAbility = "자신의 HP가 1일때만 사용할 수 있다. 서로의 소횐수들을 전부 소멸시킨다";
        cardTargetType = ECardTargetType.Selected;
    }

}