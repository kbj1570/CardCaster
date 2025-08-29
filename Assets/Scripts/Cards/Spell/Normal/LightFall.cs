using System.Collections;

public class LightFall : SpellCardData
{
    public LightFall()
    {
        cardNum = "";
        cardName = "빛의 추락";
        cardCost = 0;
        cardType = ECardType.Spell;
        cardStoryDesc = "달의 기사는 왕국을 지키기 위해 어둠에 맞선다.";
        cardDesc = "소환된 빛 속성 소환수들은 포스를 1 얻는다.";
        cardTargetType = ECardTargetType.Select;
		spellType = ESpellType.Normal;
    }

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{

		yield return null;
	}
}