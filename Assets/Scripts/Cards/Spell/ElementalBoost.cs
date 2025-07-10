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
		cardCost = 2;
		cardType = ECardType.Spell;
		cardStoryDesc = "";
		cardDesc = "자신의 소환수들의 속성의 종류 수만큼 자신의 소환수들은 포스를 얻는다";
		cardTargetType = ECardTargetType.Selected;
		
		preRequisites = new();
		PreRequisite preRequisite = new();
		preRequisite.preRequisite = EPreRequisite.PlayerServentCountOver;
		preRequisite.count = 0;
		preRequisite.serventAttribute = EServentAttribute.None;

		preRequisites.Add(preRequisite);

	}

	public override bool IsSpellUsable(BattleManager bm)
	{return true;}

	public override IEnumerator SpellEffectExecute(BattleManager bm)
	{
		List<EServentAttribute> attributes = new();
		List<Field> playerFields = bm.GetPlayerFields();

		foreach (Field field in playerFields)
		{
			if (!field.GetFilled()) continue;

			if (!attributes.Contains(field.GetServentAttribute()))
			{ attributes.Add(field.GetServentAttribute()); }
		}

		foreach (Field field in playerFields)
		{
			if (!field.GetFilled()) continue;

			field.GainForce(attributes.Count);
		}

		yield return null;
	}

}
public enum ETrigger
{
	None,
	Attack, // 공격시
	Block, // 방어시
	Hit // 피격시
}

public enum EPreRequisite
{
	None, // 무조건 사용 가능
	HandCount, // 패의 매수
	TrashCount, // 트래쉬의 매수
	DeckCount, // 덱의 매수
	HandCountOver, // 패의 매수 ~이상
	TrashCountOver,// 트래쉬의 매수 ~이상
	DeckCountOver,// 덱의 매수 ~이상
	PlayerHPCount,//나의 체력
	PlayerHPCountOver,//나의 체력 ~이상
	PlayerHPCountUnder,//나의 체력 ~이하
	HandCountUnder,// 패의 매수 ~이하
	TrashCountUnder, //트래쉬의 매수 ~이하
	DeckCountUnder, //덱의 매수 ~이하
	PlayerServentCount,//나의 소환수의 수
	PlayerServentCountUnder, //나의 소환수의 수 ~미만
	PlayerServentCountOver, //나의 소환수의 수 ~초과
	EnemyServentCount,//상대의 소환수의 수
	EnemyServentCountUnder, //상대의 소환수의 수 ~미만
	EnemyServentCountOver, //상대의 소환수의 수 ~초과
	EnemyHP, //상대의 체력
	EnemyHPOver, //상대의 체력 ~초과
	EnemyHPUnder, //상대의 체력 ~미만
	AllServentCount, // 소환수의 수
	AllServentCountOver, //소환수의 수 ~초과
	AllServentCountUnder, //소환수의 수 ~미만
	SelectedServent, // 내가 선택한 소환수
}

public struct AbilityPreRequisite
{
	public EPreRequisite preRequisite;
	public EServentAttribute serventAttribute;
	public int count;
	public string name;
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
