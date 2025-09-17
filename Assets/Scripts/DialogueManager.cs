using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class DialogueManager : MonoBehaviour
{
	public TMP_Text speakerText;
	public TMP_Text dialogueText;
	public TMP_Text documentText;
	public GameObject documentFrame;

	public GameObject choiceButtonPrefab;
	public Transform choiceContainer;
	public Transform alertPosition;
	public GameObject textBox;
	private DialogueData currentData;
	public List<DialogueData> dialogueList;
	int currentIndex = 0;
    private ILockable lockTarget;
	public GameObject alertObject;
	public GameObject FadeImage;
	public Image portraitImage;
	public Image portraitFrame;
	public Image situationImage;
	public Image fadeImage;
	public Image backgroundImage;
	public Sprite[] portraits;
	public Sprite[] situationSprites;
	bool isActionDone;
	float typingSpeed = 0.04f;
	bool isTyping = false;

	public static DialogueManager Inst { get; private set; }
	void Start()
	{
		situationImage.color = new Color(1, 1, 1, 0);
		transform.localScale = Vector3.zero;
	}

	void Awake()
	{Inst = this;}

	public void StartDialogue(int dialogueId)
	{
		lockTarget?.LockControl();
		transform.localScale = Vector3.one;
		if (dialogueId < 0 || dialogueId >= dialogueList.Count)
		{
			Debug.LogError("Denied Dialogue ID: " + dialogueId);
			return;
		}

		currentData = dialogueList[dialogueId];
		currentIndex = 0;
		StartCoroutine(ShowLine());
	}
	IEnumerator ShowLine()
	{
		if (currentData == null) yield break;
		DialogueNode line = currentData.lines[currentIndex];
		ClearChoices();
		
		documentFrame.SetActive(false);
		isActionDone = false;
		
		if(line.background != null)
		{ backgroundImage.sprite = line.background; }

		if (!string.IsNullOrEmpty(line.text))
			{
				textBox.SetActive(true);
				speakerText.text = line.speaker;
				dialogueText.text = "";
				portraitFrame.gameObject.SetActive(line.portrait != null);
				portraitImage.sprite = line.portrait;



				yield return StartCoroutine(TypeText(line.text));
				yield return new WaitUntil(() => isActionDone);
			}
			else
			{
				textBox.SetActive(false);
			}


		if (line.choices != null && line.choices.Length > 0)
		{
			foreach (var choice in line.choices)
			{
				GameObject btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
				btnObj.GetComponentInChildren<TMP_Text>().text = choice.choiceText;
				btnObj.GetComponent<Button>().onClick.AddListener(() => OnChoiceSelected(choice));
			}
		}
		else
		{
			StartCoroutine(NextLine());
		}
	}

	private IEnumerator TypeText(string text)
	{
		isTyping = true;
		dialogueText.text = "";

		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
			{
				dialogueText.text = text.Substring(0, i + 2);
				i++;
			}
			else
			{dialogueText.text = text.Substring(0, i + 1);}

			if (text[i] == '.' || text[i] == '!' || text[i] == '?')
			{typingSpeed = 0.17f;}
			else
			{typingSpeed = 0.05f;}

			yield return new WaitForSeconds(typingSpeed);
		}

		isTyping = false;
		dialogueText.text = text;
	}


	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (!isTyping)
			{isActionDone = true;}
		}
	}

	IEnumerator ExecuteEvent(DialogueEvent evt)
	{
		if (evt == null) yield return null;

		switch (evt.eventType)
		{
			case DialogueEventType.None:
				break;

			case DialogueEventType.CloseDialogue:
				lockTarget?.UnlockControl();
				transform.localScale = Vector3.zero;
				break;

			case DialogueEventType.LoadScene:
				StartCoroutine(LoadScene(evt.parameter));
				break;

			case DialogueEventType.StartDialogue:
				// DialogueData nextData = Resources.Load<DialogueData>(evt.parameter);
				StartDialogue(Int32.Parse(evt.parameter));
				break;

			case DialogueEventType.AlertUnexpectedSituation:
				yield return StartCoroutine(AlertUnexpected(evt.parameter));
				break;
			case DialogueEventType.FadeInSituationImage:
				yield return StartCoroutine(FadeInSituationImage());
				break;
			case DialogueEventType.FadeInOut:
				yield return StartCoroutine(FadeInOut());
				break;

			case DialogueEventType.FadeIn:
				yield return StartCoroutine(FadeIn());
				break;

			case DialogueEventType.FadeOut:
				yield return StartCoroutine(FadeOut());
				break;

			case DialogueEventType.GetItem:
				yield return StartCoroutine(GetItem(evt.parameter));
				break;

			case DialogueEventType.GetGold:
				yield return StartCoroutine(GetGold(evt.parameter));
				break;

			case DialogueEventType.ShowDocument:
				documentFrame.SetActive(true);
				documentText.text = evt.parameter;
				yield return new WaitUntil(() => isActionDone);
				documentFrame.SetActive(false);
				break;
		}
	}

	private IEnumerator GetGold(string parameter)
	{
		PlayerData.saveData.gold += Int32.Parse(parameter);
		yield return null;
    }

	private IEnumerator GetItem(string parameter)
    {

		// ItemData item = DataController.Inst.LoadItemDatabase()[Int32.Parse(parameter)];
		if(PlayerData.saveData.inventory_items.Count <= 8)
		{PlayerData.saveData.inventory_items.Add(parameter);}
		else
		{PlayerData.saveData.storage_items.Add(parameter);}

		yield return null;
    }

    private IEnumerator AlertUnexpected(string situationName)
	{
		textBox.SetActive(false);
		GameObject onMessage = Instantiate(alertObject, this.transform);
		onMessage.transform.position = alertPosition.position;
		onMessage.GetComponent<AlertMessage>().SetText(situationName);
		StartCoroutine(onMessage.GetComponent<AlertMessage>().FadeInOut());
		yield return new WaitForSeconds(3.8f);
		textBox.SetActive(true);
	}


	private IEnumerator LoadScene(string value)
	{
		fadeImage.gameObject.SetActive(true);

		float time = 0f;
		Color color = fadeImage.color;
		color.a = 0f;
		fadeImage.color = color;

		// Fade In
		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Clamp01(time / 1f);
			fadeImage.color = color;
			yield return null;
		}
		color.a = 1f;
		fadeImage.color = color;
		
		SceneManager.LoadScene(value);
	}

	private IEnumerator FadeOut()
	{
		fadeImage.gameObject.SetActive(true);
		float time = 0f;
		Color color = fadeImage.color;
		color.a = 0f;
		fadeImage.color = color;

		time = 0f;
		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Clamp01(1f - time / 1f);
			fadeImage.color = color;
			yield return null;
		}
		color.a = 0f;
		fadeImage.color = color;
		
		fadeImage.gameObject.SetActive(false);
	}

	private IEnumerator FadeIn()
	{
		fadeImage.gameObject.SetActive(true);
		float time = 0f;
		Color color = fadeImage.color;
		color.a = 0f;
		fadeImage.color = color;

		// Fade In
		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Clamp01(time / 1f);
			fadeImage.color = color;
			yield return null;
		}
		color.a = 1f;
		fadeImage.color = color;
	}

	private IEnumerator FadeInOut()
	{
		fadeImage.gameObject.SetActive(true);

		float time = 0f;
		Color color = fadeImage.color;
		color.a = 0f;
		fadeImage.color = color;

		// Fade In
		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Clamp01(time / 1f);
			fadeImage.color = color;
			yield return null;
		}
		color.a = 1f;
		fadeImage.color = color;

		yield return new WaitForSeconds(2f);

		// Fade Out
		time = 0f;
		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Clamp01(1f - time / 1f);
			fadeImage.color = color;
			yield return null;
		}
		color.a = 0f; // ����
		fadeImage.color = color;

		fadeImage.gameObject.SetActive(false);
	}

	private IEnumerator FadeInSituationImage()
	{

		textBox.SetActive(false);
		float alpha = 0f;
		float t = 0f;
		float fadeDuration = 0.6f; // ��Ÿ���ų� ������� �� �ɸ��� �ð�
		float stayDuration = 2.2f; // �� ��Ÿ�� �� ���� �ð�

		// Fade In
		while (t < fadeDuration)
		{
			t += Time.deltaTime;
			alpha = Mathf.Clamp01(t / fadeDuration);

			Color msgColor = situationImage.color;
			msgColor.a = alpha;
			situationImage.color = msgColor;
			yield return null;
		}

		// ���
		yield return new WaitForSeconds(stayDuration);

		textBox.SetActive(true);
	}



		IEnumerator NextLine()
		{
			yield return StartCoroutine(ExecuteEvent(currentData.lines[currentIndex].lineEvent));
			currentIndex++;
			if (currentIndex < currentData.lines.Length)
			{ StartCoroutine(ShowLine());}
			else
			{EndDialogue();}
		}

	void OnChoiceSelected(DialogueChoice choice)
	{
		StartCoroutine(ExecuteEvent(choice.choiceEvent));
		currentIndex = choice.nextDialogueIndex;
		StartCoroutine(ShowLine());
	}

	IEnumerator EndDialogue()
	{
		yield return StartCoroutine(ExecuteEvent(new DialogueEvent { eventType = DialogueEventType.CloseDialogue }));
	}

	void ClearChoices()
	{
		if (choiceContainer == null) return;

		foreach (Transform child in choiceContainer)
		{Destroy(child.gameObject);}
	}

	public void SetLockTarget(ILockable target)
	{
		lockTarget = target;
	}

}

