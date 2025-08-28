using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BattleManagerAlt : MonoBehaviour
{
	private bool phaseFlag;
	private bool mainActionFlag;
	private bool subActionFlag;
	private bool turnFlag;

	private List<Card> activatingCards;

	private Card fieldCard;



	public IEnumerator EndPhase()
	{
		/*
		 * 턴 종료시 효과들을 처리하는 단계
		 * 필드 효과, 소환수에게 걸려있는 상태 효과, 소환수 효과, 마법 효과 순서대로 처리된다.
		 */

		foreach(var card in activatingCards)
		{

			yield return new WaitUntil(() => mainActionFlag);
			//StartCoroutine(card.GetCardData());
		}

		yield return new WaitForSeconds(1f);
		Debug.Log("End Phase");
	}

	public IEnumerator DrawPhase()
	{
		/*
		 * 패가 5장이 될 때까지 드로우하고 드로우페이즈를 마친다.
		 * 만약 덱이 0장이라면 대신 묘지에서 카드를 드로우하고,
		 * 묘지에서 카드를 드로우 할 때마다 1 대미지를 받는다.
		 * 덱과 묘지가 모두 0장이라면 드로우를 진행하지 않고 드로우 페이즈를 마친다.
		*/
		yield return new WaitForSeconds(1f);
		Debug.Log("End Phase");
	}

	public IEnumerator StandByPhase()
	{
		/*
		 * 턴 시작시 효과들을 처리하는 단계
		 * 필드 효과, 소환수에게 걸려있는 상태 효과, 소환수 효과, 마법 효과 순서대로 처리된다.
		 * 처리할 효과가 없거나 효과를 모두 처리하면 스탠바이 페이즈를 마친다
		 */
		yield return new WaitForSeconds(1f);
		Debug.Log("End Phase");
	}

	public IEnumerator Attack()
	{
		/*
		 * 대상 지정 후 공격 선언
		 * 서로가 받는 대미지 계산 완료
		 * 계산된 대미지가 서로에게 동시에 적용
		 * 공격자가 받는 대미지가 1 이상이라면 공격자의 대미지를 입었을 시 효과 적용
		 * 방어자의 피격 시 효과 적용
		 * 방어자가 받는 대미지가 1 이상이라면 방어자의 대미지를 입었을 시 효과가 적용
		 * 공격 종료
		 * 소멸처리
		 * 공격자의 소멸 시 효과 적용
		 * 방어자의 소멸 시 효과 적용
		 */
		yield return new WaitForSeconds(1f);
	}

	public IEnumerator MainPhase()
	{
		/* 플레이어가 행동하는 단계
		 * 행동을 모두 마치고 메인 페이즈를 마친다.
		 * 행동이란 소환, 마법 사용, 공격, 패 버리기 등을 포함한다.
		 * 행동을 모두 마쳤다면 메인 페이즈를 마친다.
		 */
		yield return new WaitForSeconds(1f);
		Debug.Log("End Phase");
	}

	public IEnumerator GameLoop()
	{
		while (true)
		{
			StartCoroutine(PlayerTurn());
			yield return new WaitUntil(() => turnFlag);
			turnFlag = false;
			StartCoroutine(EnemyTurn());
			yield return new WaitUntil(() => turnFlag);
			turnFlag = false;
		}
	}

	public IEnumerator PlayerTurn()
	{
		StartCoroutine(DrawPhase());
		yield return new WaitUntil(() => phaseFlag);
		StartCoroutine(StandByPhase());
		yield return new WaitUntil(() => phaseFlag);
		StartCoroutine(MainPhase());
		yield return new WaitUntil(() => phaseFlag);
		StartCoroutine(EndPhase());
		yield return new WaitUntil(() => phaseFlag);
	}

	public IEnumerator EnemyTurn()
	{

		StartCoroutine(StandByPhase());
		yield return new WaitUntil(() => phaseFlag);
		StartCoroutine(MainPhase());
		yield return new WaitUntil(() => phaseFlag);
		StartCoroutine(EndPhase());
		yield return new WaitUntil(() => phaseFlag);
	}
}