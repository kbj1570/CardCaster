using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
	CutScenes cutScenes;
	public GameObject textBoxObject;
	public TMP_Text textBox;
	public TMP_Text nameBox;
	public AudioSource soundManager;

	public string cutsceneNum;
	public static CutSceneManager Inst { get; private set; }

	public List<Sprite> characters;


	public List<NamedSprite> characterList;  // ✅ 인스펙터에서 key + image 세트로 보임

	private Dictionary<string, Sprite> characterMap;

	public List<AudioClip> soundEffects;
	public List<AudioClip> backgroundMusic;

	public Dictionary<string, Dialogue> dialogues;

	public Image characterOnLeftSide;
	public Image characterOnRightSide;
	public Image fadeImage;
	private StringBuilder currentText = new StringBuilder();

	List<CutSceneNode> cutSceneNodes;

	bool isActionDone;
	float typingSpeed = 0.04f;
	bool isTyping = false;

	void Awake()
	{
		Inst = this;
		dialogues = DataController.Inst.LoadDialogues("dialogues_ko.json");
		cutSceneNodes = DataController.Inst.LoadCutScene(cutsceneNum);


		characterMap = new Dictionary<string, Sprite>();

		foreach (NamedSprite entry in characterList)
		{
			if (!string.IsNullOrEmpty(entry.name) && entry.sprite != null)
			{
				characterMap[entry.name] = entry.sprite;
			}
		}

		//StartCoroutine(StartCutScene());

	}

	public void ClearCutScene()
	{

	}


	[System.Serializable]
	public class NamedSprite
	{
		public string name;   
		public Sprite sprite;   
	}


	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(!isTyping)
			isActionDone = true;
		}
	}

	public void LoadCutScene(int cutSceneNum)
	{

	}



	public IEnumerator StartCutScene()
	{
		foreach (CutSceneNode cutSceneNode in cutScenes.GetCutSceneNodes())
		{
			isActionDone = false;
			switch (cutSceneNode.commandType)
			{
				case ECommandType.Wait: // ~초 기다리기

					float.TryParse(cutSceneNode.parameters, out float waitTime);
					yield return new WaitForSeconds(waitTime);
					break;

				case ECommandType.ShowText:
					isTyping = true;
					Dialogue dialogue = dialogues[cutSceneNode.parameters];


					textBoxObject.SetActive(true);
					nameBox.text = dialogue.speaker;
					textBox.text = dialogue.text;

					for (int i = 0; i < dialogue.text.Length; i++)
					{
						if (dialogue.text[i] == '.' ||
						dialogue.text[i] == '!' ||
						dialogue.text[i] == '?')
						{ typingSpeed = 0.17f; }
						else
						{ typingSpeed = 0.05f; }
						textBox.text = dialogue.text.Substring(0, i + 1); // 한 글자씩 추가
						yield return new WaitForSeconds(typingSpeed);



					}

					isTyping = false;

					yield return new WaitUntil(() => isActionDone);
					break;

				case ECommandType.HideText:
					textBoxObject.SetActive(false);
					break;

				case ECommandType.ShowCharacterLeftSide: // 왼쪽에 캐릭터 띄우기
					characterOnLeftSide.sprite = characterMap[cutSceneNode.parameters];

					if (!characterOnLeftSide.gameObject.activeSelf)
					{
						characterOnLeftSide.gameObject.SetActive(true);
						float time = 0;
						Color color = characterOnLeftSide.color;

						while (time < 0.7f)
						{
							time += Time.deltaTime;
							color.a = Mathf.Lerp(0, 1, time / 0.7f);
							characterOnLeftSide.color = color;
							yield return null;
						}
					}
					break;

				case ECommandType.HideCharacterLeftSide:
					{
						float time = 0.7f;
						Color color = characterOnLeftSide.color;

						while (time > 0)
						{
							time += Time.deltaTime;
							color.a = Mathf.Lerp(1, 0, time / 0.7f);
							characterOnLeftSide.color = color;
							yield return null;
						}
						characterOnLeftSide.gameObject.SetActive(false);
						break;
					}

				case ECommandType.HighLightCharacterLeftSide:
					characterOnRightSide.color = new Color(0.5f, 0.5f, 0.5f);
					characterOnLeftSide.color = new Color(1f, 1f, 1f);
					break;


				case ECommandType.ShowCharacterRightSide: // 오른쪽에 캐릭터 띄우기
					characterOnRightSide.sprite = characterMap[cutSceneNode.parameters];

					if (!characterOnRightSide.gameObject.activeSelf)
					{
						characterOnRightSide.gameObject.SetActive(true);
						float time = 0;
						Color color = characterOnRightSide.color;

						while (time < 0.7f)
						{
							time += Time.deltaTime;
							color.a = Mathf.Lerp(0, 1, time / 0.7f); // 알파 값을 0 → 1로 변경
							characterOnRightSide.color = color;
							yield return null;
						}
					}
					// characterOnRightSide.gameObject.SetActive(true);

					break;

				case ECommandType.HideCharacterRightSide: // 오른쪽에 캐릭터 숨기기
					break;

				case ECommandType.FadeOutScreen: // 화면 어둡게
					{
						float time = 0;
						Color color = fadeImage.color;
						float.TryParse(cutSceneNode.parameters, out float duration);

						while (time < duration)
						{
							time += Time.deltaTime;
							color.a = Mathf.Lerp(0, 1, time / duration); 
							fadeImage.color = color;
							yield return null;
						}
						break;

					}


				case ECommandType.FadeInScreen: // 화면 밝게
					{
						float time = 0;
						Color color = fadeImage.color;
						float.TryParse(cutSceneNode.parameters, out float duration);

						while (time < duration)
						{
							time += Time.deltaTime;
							color.a = Mathf.Lerp(1, 0, time / duration); // 알파 값을 1 → 0으로 변경
							fadeImage.color = color;
							yield return null;
						}
						break;

					}
				case ECommandType.HighLightCharacterRightSide: // 오른쪽에 띄운 캐릭터를 강조
					characterOnLeftSide.color = new Color(0.5f, 0.5f, 0.5f);
					characterOnRightSide.color = new Color(1f, 1f, 1f);
					break;
			}

		}

		yield return null;
	}

}