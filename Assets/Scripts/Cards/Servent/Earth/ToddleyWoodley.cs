using System.Collections;

public class ToddleyWoodley : ServentCardData
{
    public ToddleyWoodley()
    {
        cardNum = "119";
        cardName = "우들리&토들리";
        cardCost = 2;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 5;
        cardStoryDesc = "";
        cardDesc = "공격 시 자신은 HP를 3 잃는다.";
		serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Earth;
    }
	public override IEnumerator AttackEffectExecute(BattleManager bm)
	{
		bm.PlayerTakeDamage(3);
		yield return null;
	}
}
