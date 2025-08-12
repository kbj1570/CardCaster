using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class VioletLichLord : ServentCardData
{
    public VioletLichLord()
    {
        cardNum = "4";
        cardName = "바이올렛 리치로드";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        serventAttribute = EServentAttribute.Dark;
        force = 1;
        cardStoryDesc = "";
        cardDesc = "소환시 묘지에서 원하는 마법카드를 1장 가져온다";
        cardTargetType = ECardTargetType.Select;
        
        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.TrashCountOver;
        preRequisite.count = 0;
        preRequisite.cardType = ECardType.Spell;
        preRequisites.Add(preRequisite);
    }
	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		//ShowSelectedCards(trashList, ECardType.Spell, 1);
		//yield return new WaitUntil(() => isActionDone);

		//SpellCardData card = selectedCards[0] as SpellCardData;
		//RemoveTrash(card);
		//cardPrefab = cardPrefabList[card.GetCardNum()];

		//GameObject cardObject = Instantiate(cardPrefab, new Vector3(), Utils.QI);
		//cardObject.transform.SetParent(canvas.transform);
		//cardObjectList.Add(cardObject);

		//cardObject.GetComponent<BattleCardObject>().Setup(card);

		//cardObject.GetComponent<BattleCardObject>().SetCardOrder(handList.Count);
		//handList.Add(card);

		//selectedCards = new();

		//isActionDone = false;

		//CardAlignmentAlt();

		//ShotDrawMissile(cardObject.transform);
		yield return null;
	}
	public override IEnumerator AttackEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override IEnumerator DefendEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override bool IsAbilityUsable(BattleManager bm)
	{
		return true;
	}
}