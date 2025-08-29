using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class Griffin : ServentCardData
{
    public Griffin()
    {
        cardNum = "114";
        cardName = "그리핀";
        cardCost = 2;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 4;
		cardStoryDesc = "성격은 온화하고 조용히 지내며, 자신이 인정한 존재에게 충성한다.";
        cardDesc = "소환 시 자신의 바람 속성 소환수 수만큼 드로우한다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Wind;
    }

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{

		List<EServentAttribute> attributes = new();
		List<Field> playerFields = bm.GetPlayerFields();

		int count = 0;

		foreach (Field field in playerFields)
		{
			if (!field.GetFilled()) continue;

			if (field.GetServentAttribute() == EServentAttribute.Wind)
			{count++;}
		}

		for(int i = 0; i < count; i++)
		{bm.DrawCard();}

		yield return null;
	}
}
