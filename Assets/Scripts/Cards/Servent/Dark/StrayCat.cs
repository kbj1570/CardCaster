using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;

public class StrayCat : ServentCardData
{
    public StrayCat()
    {
        cardNum = "124";
        cardName = "도둑고양이";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 1;
        cardStoryDesc = "";
        cardDesc = "소환시 가진 골드에 따라 포스를 얻는다. (100G / 1)";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Targeting;
        serventAttribute = EServentAttribute.Dark;
    }

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
        Servent servent = null;
		servent.GainForce(PlayerData.saveData.gold / 100);
		yield return null;
	}
}