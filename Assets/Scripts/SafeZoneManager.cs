using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SafeZoneManager : MonoBehaviour
{
	public GameObject textBoxObject;
	public TMP_Text textBox;
	public TMP_Text nameBox;
	public AudioSource soundManager;
	public SafeZone safeZone;


	public List<Sprite> characters;

	public List<AudioClip> soundEffects;
	public List<AudioClip> backgroundMusic;

	public Image characterOnLeftSide;
	public Image characterOnRightSide;
	public Image fadeImage;

	public GameObject commentaryPreFab;
	public Transform commentaryLocation;

	void Start()
	{
		safeZone = new CabinSafeZone();

		StartCoroutine(FadeIn());
	}
	public void Heal()
	{
		PlayerData.saveData.health = 30;
		GameObject onMessage = Instantiate(commentaryPreFab, commentaryLocation);
		onMessage.GetComponent<PopUpMessage>().SetText("체력이 전부 회복되었다.");
	}

	public void OpenStorage()
	{

	}

	public void ShowCommentary(int value)
	{
		GameObject onMessage = Instantiate(commentaryPreFab, commentaryLocation);
		onMessage.GetComponent<PopUpMessage>().SetText(safeZone.GetCommentaries()[value]);
	}

	public void SaveData()
	{
		DataController.Inst.SaveData(PlayerData.saveData);
		Debug.Log("데이터를 저장했습니다.");
	}

	public void LoadCamp()
	{
		StartCoroutine(BackToCampsite());
	}

	IEnumerator BackToCampsite()
	{
		DungeonData.Reset();
		StartCoroutine(FadeOut());
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("Campsite");
	}

	public void ShowCutScene(string cutSceneNum)
	{
		StartCoroutine(ShowCutSceneCoroutine(cutSceneNum));
	}

	IEnumerator ShowCutSceneCoroutine(string cutSceneNum)
	{
		if (!PlayerData.saveData.cutsceneVIewed[cutSceneNum])
		{
			switch (cutSceneNum)
			{
				case "0":
					StartCoroutine(FadeOut());
					yield return new WaitForSeconds(1f);
					SceneManager.LoadScene("HowAboutTrade");
					break;

				case "2":
					StartCoroutine(FadeOut());
					yield return new WaitForSeconds(1f);
					SceneManager.LoadScene("SmallTalk");
					break;
			}
		}
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

	private IEnumerator FadeOut()
	{
		fadeImage.gameObject.SetActive(true);
		float time = 0;
		Color color = fadeImage.color;

		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Lerp(0, 1, time / 1f);
			fadeImage.color = color;
			yield return null;
		}
	}
}
