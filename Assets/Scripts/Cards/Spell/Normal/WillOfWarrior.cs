using System.Collections;

public class WillOfWarrior : SpellCardData
{
    public WillOfWarrior()
    {
        cardNum = "108";
        cardName = "투사의 의지";
        cardCost = 2;
        cardType = ECardType.Spell;
        cardStoryDesc = "달의 기사는 왕국을 지키기 위해 어둠에 맞선다.";
        cardDesc = "소환수 하나의 포스를 2배로 한다, 그 소환수는 턴이 끝나면 소멸한다.";
        cardTargetType = ECardTargetType.Targeting;
		spellType = ESpellType.Normal;
    }

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
		yield return null;
	}

}