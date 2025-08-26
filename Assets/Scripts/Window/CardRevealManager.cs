using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardRevealManager : MonoBehaviour
{
	public Transform cardParent;          // 카드 생성 위치 (Canvas 안의 빈 오브젝트)
	public GameObject cardPrefab;         // Shard + Card 연출 프리팹
	public float delayBetweenCards = 0.5f;
	public GameObject confirmButton;      // 확인 버튼

	private void Start()
	{
		confirmButton.SetActive(false);
		StartCoroutine(RevealSequence());
	}

	private IEnumerator RevealSequence()
	{
		foreach (var card in CardRevealData.revealedCards)
		{
			GameObject cardObj = Instantiate(cardPrefab, cardParent);
			Card cardUi = cardObj.GetComponent<Card>();

			cardUi.Setup(card);

			yield return StartCoroutine(cardUi.PlayRevealAnimation());

			yield return new WaitForSeconds(delayBetweenCards);
		}

		// 모든 카드 다 끝나면 확인 버튼 표시
		confirmButton.SetActive(true);
	}

	// 확인 버튼이 눌리면 메인 씬으로 복귀
	public void OnConfirm()
	{
		SceneManager.LoadScene("MainScene");
	}
}

public static class CardRevealData
{
	public static List<CardData> revealedCards = new List<CardData>();
}