using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public interface ILockable
{
	void LockControl();
	void UnlockControl();
}
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
	public static string nextScene;
}

public static class DungeonClearData
{
	public static Enemy enemy;
}

public class CampsiteManager : MonoBehaviour, ILockable
{
	public Image fadeImage;
	public AudioSource audioSource;
	public AudioClip inventoryOpen;
	public AudioClip inventoryClose;
	public AudioClip mapOpen;

	List<CardData> commonCards;
	List<CardData> uncommonCards;
	List<CardData> rareCards;

	public Window dungeonSelectWindow;
	public Window storageWindow;
	public Window randomBoxWindow;

	public bool screenLocked = false;

	public static CampsiteManager Inst { get; private set; }


	void Start()
	{
		// PlayerData.saveData = DataController.Inst.LoadData();
		DeckManager.Inst.LoadDeck();
		Inst = this;
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


	public void PlayInventoryOpen()
	{audioSource.PlayOneShot(inventoryOpen);}

	public void PlayInventoryClose()
	{audioSource.PlayOneShot(inventoryClose);}

	public void PlayMapOpen()
	{
		audioSource.PlayOneShot(mapOpen);
		
	}

	public void OpenMap()
	{
		dungeonSelectWindow.OnOff();
	}

	public void LockScreen(bool value)
	{
		screenLocked = value;
	}

	public void LockControl()
	{screenLocked = true;}

	public void UnlockControl()
	{screenLocked = false;}

	public void StartDialogue()
	{
	}


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

	public void OpenStorage()
	{
		storageWindow.GetComponent<StorageWindow>().UpdateItemPage();
		storageWindow.OnOff();
	}

	public void OpenRandomBox()
	{
		//storageWindow.GetComponent<RandomBoxWindow>().UpdateItemPage();
		randomBoxWindow.OnOff();
	}

	public void GoToShop()
	{SceneManager.LoadScene("Shop");}

	public void GoToCamp()
	{SceneManager.LoadScene("Camp");}

	public void GoToCollector()
	{SceneManager.LoadScene("OldCabin");}
}
