using System.Collections;
using UnityEngine;

public class FireBat : ServentCardData
{
	public FireBat()
	{
		cardNum = "123";
		cardName = "화염박쥐";
		cardCost = 2;
		cardType = ECardType.Servent;
		serventType = EServentType.Player;
		force = 2;
		cardStoryDesc = "작은 박쥐의 형태를 하고 있으며, 몸 전체에 화염을 두르고 있다.\r\n동굴이나 용암 지대에 서식하며, 침입자를 불길에 휩싸인 발톱으로 공격한다\r\n멀리서 보면 공중을 떠다니는 불덩이처럼 보여, 도깨비불로 착각하는 여행자들도 있다고 한다.";
		cardDesc = "소환 시 상대에게 2 대미지를 주고, 자신은 2 회복한다.";
		serventSize = EServentSize.Small;
		cardTargetType = ECardTargetType.Select;
		serventAttribute = EServentAttribute.Fire;
	}

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		bm.DealDamageToEnemy(2);
		bm.HealPlayer(2);
		yield return new WaitForSeconds(1f);
	}
}