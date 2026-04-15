using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ElementalBoost : SpellCardData
{
	
	public ElementalBoost()
	{
		cardNum = "1";
		cardName = "엘리멘탈 부스트";
		cardCost = 3;
		cardType = ECardType.Spell;
		cardStoryDesc = "생명이 빛이 꺼져가는 기사의 주위에 작은 생명들의 모이기 시작했다. 작고 따뜻한 생명들의 기운이 모여 기사의 상처를 치유한다.";
		cardDesc = "자신의 소환수들의 속성의 종류 수만큼 자신의 소환수들은 전부 포스를 얻는다";
		cardTargetType = ECardTargetType.NoneTargeting;

	}

	public override bool IsCardUsable(BattleManager bm)
	{return true;}

	public override IEnumerator ActivationEffectExecute(BattleManager bm)
	{
		List<EServentAttribute> attributes = new();
		List<Servant> playerServents = bm.GetServents(EServentType.Player);

		foreach (Servant servent in playerServents)
		{
			if (!attributes.Contains(servent.GetAttribute()))
			{ attributes.Add(servent.GetAttribute()); }
		}

		foreach (Servant servent in playerServents)
		{
			servent.GainForce(attributes.Count);
		}

		yield return null;
	}
}



// Verb 소환된 내 소환수가 있을 때
// 무슨 속성?
// 몇 마리?

//Verb 소환된 상대 소환수가 있을 때
//무슨 속성?
// 몇 마리?

// 소환된 내 소환수가 몇마리 이상 있을 때
// 무슨 속성?
// 몇 마리?

// 내 패가 몇 장일 때
