using System.Collections;

public class NamelessTraveler : ServentCardData
{
    public NamelessTraveler()
    {
        cardNum = "127";
        cardName = "무명의 사자";
        cardCost = 3;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 3;
        cardStoryDesc = "";
        cardDesc = "다른 소환수를 공격할 시, 모든 적에게 1 대미지를 준다.";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Dark;
    }
	public override IEnumerator AttackEffectExecute(BattleManager bm)
	{
		yield return null;
	}
}