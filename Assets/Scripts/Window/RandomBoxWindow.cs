using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class RandomBoxWindow : Window
{

	private List<CardData> normalCardList;
	private List<CardData> rareCardList;

	public TMP_Text shardText;

	void Start()
	{
		normalCardList = DataController.Inst.LoadNormalCard();
		rareCardList = DataController.Inst.LoadRareCard();
		shardText.text = PlayerData.saveData.shard.ToString();
		ScaleZero();
	}


	public void OpenRandomBox()
	{

		if (PlayerData.saveData.shard > 0)
		{
			int rareCheck = Random.Range(0, 100);

			CardData randomCard;

			if (rareCheck < 10)
			{ randomCard = rareCardList[Random.Range(0, rareCardList.Count)]; }
			else
			{ randomCard = normalCardList[Random.Range(0, normalCardList.Count)]; }

			if (PlayerData.saveData.cardList.TryGetValue(randomCard.GetCardName(), out int count))
			{ PlayerData.saveData.cardList[randomCard.GetCardName()] = count + 1; }
			else
			{ PlayerData.saveData.cardList[randomCard.GetCardName()] = 1; }


			Debug.Log("획득한 카드 : " + randomCard.GetCardName() + " / 보유 개수 : " + PlayerData.saveData.cardList[randomCard.GetCardName()]);
			PlayerData.saveData.shard--;
			shardText.text = PlayerData.saveData.shard.ToString();
		}
		else
		{ Debug.Log("파편이 부족합니다."); }
	}
}
