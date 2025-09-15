using System.Collections;
using System.Collections.Generic;

public class Georgius : ServentCardData
{
    public Georgius()
    {
        cardNum = "115";
        cardName = "제오르기우스";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 3;
        cardStoryDesc = "언제나 전투의 선봉에 서서 용감하게 싸워나가는 전사.\r\n정의로운 심성과 올곧은 의지를 지닌 전사는.";
        cardDesc = "소환시, 의지 상태를 갖는다.";
		serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Light;
		hasStatusEffect = true;
        statusConditions = new EStatusCondition[] { EStatusCondition.Will};
	}

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}
