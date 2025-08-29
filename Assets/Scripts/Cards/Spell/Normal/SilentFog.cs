using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilentFog : SpellCardData
{
    public SilentFog()
    {
        cardNum = "129";
        cardName = "침묵의 안개";
        cardCost = 1;
        cardType = ECardType.Spell;
        cardStoryDesc = "누군가의 비명소리가 안개속에 묻혀서 사라져 간다.\r\n짙은 안개가 사라지고 남겨진 숲에는 아무도 남아있지 않았다.";
        cardDesc = "자신의 HP가 1일때만 사용할 수 있다. 서로의 소횐수들을 전부 소멸시킨다.";
        cardTargetType = ECardTargetType.Selected;
		spellType = ESpellType.Normal;
    }

	public override bool IsCardUsable(BattleManager bm)
	{
		return bm.playerHealth == 1;
	}

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
		List<Field> allFields = bm.GetAllFields();
		foreach (Field field in allFields)
		{
            if (field.GetFilled())
                field.Kill();
		}

		yield return new WaitForSeconds(1f);
	}


}