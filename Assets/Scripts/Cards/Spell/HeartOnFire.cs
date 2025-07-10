using System.Collections;

public class HeartOnFire : SpellCardData
{
    public HeartOnFire()
    {
        cardNum = "12";
        cardName = "타오르는 심장";
        cardCost = 1;
        cardType = ECardType.Spell;
        cardStoryDesc = "";
        cardDesc = "소환수 하나의 포스를 2배로 하고 \r\n 그 소환수는 이 턴이 끝나면 소멸한다";
        cardTargetType = ECardTargetType.Select;
    
    }


	public override bool IsSpellUsable(BattleManager bm)
	{
		return true;
	}

	public override IEnumerator SpellEffectExecute(BattleManager bm)
	{
		bm.DrawCard();
		bm.DrawCard();
		bm.DrawCard();

		yield return null;
	}

}