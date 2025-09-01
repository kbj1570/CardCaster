using System.Collections;
using System.Collections.Generic;

public class CrescentLancer : ServentCardData
{
    public CrescentLancer()
    {
        cardNum = "101";
        cardName = "크레센트 랜서";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 3;
        cardStoryDesc = "달에서 추방당했다는 전설 속의 기사, 언젠간 다시 달로 돌아갈 날을 기다리며 지상을 떠돌고 있다.";
        cardDesc = "공격 시 포스가 상대 소환수의\r\n포스보다 높다면 그 차이만큼\r\n상대에게 대미지를 준다.";

		penetrate = true;
		serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Light;
    }
	public override IEnumerator AttackEffectExecute(BattleManager bm)
	{
		//bm.Pierce();
		yield return null;
	}
}
