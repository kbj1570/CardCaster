using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
	public Image maskImage;
	public Image backgroundImage;
	public Sprite[] portraits;
	public Sprite[] situationSprites;
	bool isActionDone;
	float typingSpeed = 0.04f;

	// === DialogueManager 멤버 필드에 추가 ===
	bool autoFastForward = false;      // F로 토글: 자동으로 빠르게 넘김 (선택지 있을 땐 멈춤)
	bool skipAllRequested = false;     // Ctrl+Tab: 대화 전체 스킵
	bool skipCurrentRequested = false; // Tab: 현재 줄 즉시 완성/다음으로

	// 타이핑 속도 프로파일
	readonly float typeSpeedNormal = 0.05f;
	readonly float typeSpeedPunct  = 0.17f;  // . ! ? 뒤 잠깐 쉬는 느낌
	readonly float typeSpeedFast   = 0.001f; // 빨리감기시
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

    if (line.background != null)
    {
        backgroundImage.color = new Color(1, 1, 1, 1);
        backgroundImage.sprite = line.background;
    }

    if (!string.IsNullOrEmpty(line.text))
    {
        textBox.SetActive(true);
        speakerText.text = line.speaker;
        dialogueText.text = "";
        portraitFrame.gameObject.SetActive(line.portrait != null);
        portraitImage.sprite = line.portrait;

        yield return StartCoroutine(TypeText(line.text));

        // ✅ 자동 빨리감기 중이고, 현재 줄에 선택지가 없는 경우 자동 진행
        if (autoFastForward && (line.choices == null || line.choices.Length == 0))
            isActionDone = true;

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
        // 자동 빨리감기여도 "선택"은 플레이어가 하게 둔다 (자동 선택 X)
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
		skipCurrentRequested = false; // 줄 시작 시 리셋

		for (int i = 0; i < text.Length; i++)
		{
			// 전체 스킵 중이면 즉시 종료
			if (skipAllRequested)
			{
				dialogueText.text = text;
				break;
			}

			// 현재 줄 스킵이면 즉시 완성
			if (skipCurrentRequested)
			{
				dialogueText.text = text;
				break;
			}

			// 캐리지리턴+뉴라인 처리 (원래 로직 유지)
			if (text[i] == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
			{
				dialogueText.text = text.Substring(0, i + 2);
				i++;
			}
			else
			{
				dialogueText.text = text.Substring(0, i + 1);
			}

			// 속도 결정: 빨리감기면 극단적으로 빠르게
			bool isPunct = (text[i] == '.' || text[i] == '!' || text[i] == '?');
			float delay = autoFastForward ? typeSpeedFast : (isPunct ? typeSpeedPunct : typeSpeedNormal);

			yield return new WaitForSeconds(delay);
		}

		isTyping = false;
		dialogueText.text = text;
	}


	void Update()
{
    // Space: (원래 있던 로직) 타이핑 중이 아니면 다음으로
    if (Input.GetKeyDown(KeyCode.Space))
    {
        if (!isTyping) { isActionDone = true; }
    }

    // Tab: 스킵
    if (Input.GetKeyDown(KeyCode.Tab))
    {
        if (isTyping)  // 현재 줄 타이핑 중이면 즉시 완성
            skipCurrentRequested = true;
        else           // 이미 완성된 상태면 다음 줄로Fty
            isActionDone = true;
    }

    // Ctrl+Tab: 전체 스킵
    if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
        Input.GetKeyDown(KeyCode.Tab))
    {
        skipAllRequested = true;
        SkipAllDialogue(); // 아래 헬퍼
    }

    // F: 자동 빨리감기 토글 (선택지/문서 화면에서는 자동 선택/닫기 안 함)
    if (Input.GetKeyDown(KeyCode.F))
    {
        autoFastForward = !autoFastForward;
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
			case DialogueEventType.ClearBackground:
				backgroundImage.color = new Color(1, 1, 1, 0);
				break;

			case DialogueEventType.KeyInputRight:
				DungeonManager.Inst.InputKeyEast();
				yield return new WaitForSeconds(0.8f);
				break;
			case DialogueEventType.KeyInputLeft:
				DungeonManager.Inst.InputKeyWest();
				yield return new WaitForSeconds(0.8f);
				break;
			case DialogueEventType.KeyInputUp:
				DungeonManager.Inst.InputKeyNorth();
				yield return new WaitForSeconds(0.8f);
				break;
			case DialogueEventType.KeyInputDown:
				DungeonManager.Inst.InputKeySouth();
				yield return new WaitForSeconds(0.8f);
				break;
			case DialogueEventType.ShowMaskImage:
				maskImage.gameObject.SetActive(true);
				break;
			case DialogueEventType.HideMaskImage:
				maskImage.gameObject.SetActive(false);
				break;
			case DialogueEventType.SpawnEnemy:
				DungeonManager.Inst.SpawnEnemy(evt.parameter);
				yield return new WaitForSeconds(0.1f);
				break;
			case DialogueEventType.MoveEnemyUp:
				DungeonManager.Inst.MoveEnemy(Int32.Parse(evt.parameter), EDirection.North);
				yield return new WaitForSeconds(0.4f);
				break;
			case DialogueEventType.RevealNode:
				DungeonManager.Inst.RevealNode(evt.parameter);
				break;

			case DialogueEventType.CameraFollowingON:
				CameraController.Inst.SetFollowing(true);
				yield return null;
				break;

			case DialogueEventType.CameraFollowingOFF:
				CameraController.Inst.SetFollowing(false);
				yield return null;
				break;
			case DialogueEventType.FadeOutSituationImage:
				yield return StartCoroutine(FadeOutSituationImage());
				break;

		}
	}
	private IEnumerator GetGold(string parameter)
	{
		PlayerData.saveData.gold += Int32.Parse(parameter);
		yield return null;
	}
	void SkipAllDialogue()
	{
		// 이미 엔딩 처리 중이면 무시
		if (currentData == null) return;

		// 코루틴 상태를 정리하고 바로 종료 처리
		StopAllCoroutines();
		// 텍스트/선택지 초기화
		ClearChoices();
		textBox.SetActive(false);

		// 인덱스를 끝으로 밀고 종료 코루틴 실행
		currentIndex = currentData.lines.Length;
		StartCoroutine(EndDialogue());
	}

	private IEnumerator GetItem(string parameter)
    {
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
		DOTween.KillAll();
		
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
		float fadeDuration = 0.6f; 
		float stayDuration = 2.2f;

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
		yield return new WaitForSeconds(stayDuration);

		textBox.SetActive(true);
	}


	private IEnumerator FadeOutSituationImage()
	{

		textBox.SetActive(false);
		float alpha = 1f;
		float t = 0f;
		float fadeDuration = 0.5f; 
		float stayDuration = 1.5f;

		// Fade In
		while (t < fadeDuration)
		{
			t += Time.deltaTime;
			alpha = Mathf.Clamp01(1 - t / fadeDuration);

			Color msgColor = situationImage.color;
			msgColor.a = alpha;
			situationImage.color = msgColor;
			yield return null;
		}
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

