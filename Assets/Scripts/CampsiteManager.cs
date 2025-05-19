using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class DungeonData
{
	public static Dungeon dungeon;
	public static int currentPlayerLocation;
	public static int previousPlayerLocation;
	public static List<Node> map;
	public static List<int> nodeNumList;
	public static Dictionary<int, bool> activeNodes;
	public static Dictionary<int, bool> visitedNodes;
	public static Dictionary<DungeonEnemy, int> dungeonEnemies;
	public static int currentFloor;


	public static void Reset()
	{
		dungeon = null;
		map = null;
	}
}

public static class PlayerData
{
	public static SaveData saveData;
}

public static class BattleData
{
	public static List<Enemy> enemies;
}

public static class DungeonClearData
{
	public static Enemy enemy;
}

public class CampsiteManager : MonoBehaviour
{
	public Image fadeImage;
	public AudioSource audioSource;
	public AudioClip inventoryOpen;
	public AudioClip inventoryClose;
	public AudioClip mapOpen;

	List<CardData> commonCards;
	List<CardData> uncommonCards;
	List<CardData> rareCards;

	void Start()
	{
		PlayerData.saveData = DataController.Inst.LoadData();
		StartCoroutine(FadeIn());
	}

	public void GoToDungeon()
	{
		fadeImage.gameObject.SetActive(true);
		StartCoroutine(FadeOut());
	}

	public void SaveData()
	{ 
		DataController.Inst.SaveData(PlayerData.saveData);
		Debug.Log("데이터를 저장했습니다.");
	}

	public void OpenCardPack()
	{
		ECardRarity eCardRarity;
		int randomNum = Random.Range(0, 10);

		if(randomNum < 6)
		{eCardRarity = ECardRarity.Common;}
		else if(randomNum < 9)
		{eCardRarity = ECardRarity.Uncommon;}
		else
		{eCardRarity = ECardRarity.Rare;}

		switch(eCardRarity)
		{
			case ECardRarity.None:
				break;

			case ECardRarity.Common:
				break;

			case ECardRarity.Uncommon:
				break;

			case ECardRarity.Rare:
				break;
		}

	}

	public void PlayInventoryOpen()
	{audioSource.PlayOneShot(inventoryOpen);}

	public void PlayInventoryClose()
	{audioSource.PlayOneShot(inventoryClose);}

	public void PlayMapOpen()
	{audioSource.PlayOneShot(mapOpen);}


	private IEnumerator FadeOut()
	{
		float time = 0;
		Color color = fadeImage.color;

		while (time < 0.6f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Lerp(0, 1, time / 0.6f);
			fadeImage.color = color;
			yield return null;
		}

		DungeonData.dungeon = new Graveyard();
		SceneManager.LoadScene("Dungeon");
	}

	private IEnumerator FadeIn()
	{
		float time = 0;
		Color color = fadeImage.color;

		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Lerp(1, 0, time / 1f);
			fadeImage.color = color;
			yield return null;
		}
		fadeImage.gameObject.SetActive(false);
	}

	public void GoToShop()
	{SceneManager.LoadScene("Shop");}

	public void GoToCamp()
	{SceneManager.LoadScene("Camp");}

	public void GoToCollector()
	{SceneManager.LoadScene("OldCabin");}
}
