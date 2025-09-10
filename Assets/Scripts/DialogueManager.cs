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
	public GameObject choiceButtonPrefab;
	public Transform choiceContainer;

	private DialogueData currentData;
	public List<DialogueData> dialogueList;
	int currentIndex = 0;
    private ILockable lockTarget;

	private GameObject alertObject;
	public static DialogueManager Inst { get; private set; }
	void Start()
	{
		transform.localScale = Vector3.zero;
	}
	void Awake()
	{
		Inst = this;
	}

	public void StartDialogue(int dialogueId)
	{
		lockTarget?.LockControl();
		transform.localScale = Vector3.one;
		if (dialogueId < 0 || dialogueId >= dialogueList.Count)
		{
			Debug.LogError("잘못된 Dialogue ID: " + dialogueId);
			return;
		}

		currentData = dialogueList[dialogueId];
		currentIndex = 0;
		ShowLine();
	}

	void ShowLine()
	{
		if (currentData == null) return;
		DialogueNode line = currentData.lines[currentIndex];

		ClearChoices();
		speakerText.text = line.speaker;
		dialogueText.text = line.text;

		if (line.choices != null && line.choices.Length > 0)
		{
			foreach (var choice in line.choices)
			{
				GameObject btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
				btnObj.GetComponentInChildren<TMP_Text>().text = choice.choiceText;
				btnObj.GetComponent<Button>().onClick.AddListener(() => OnChoiceSelected(choice));
			}
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			DialogueNode line = currentData.lines[currentIndex];
			if (line.choices == null || line.choices.Length == 0)
			{
				NextLine();
			}
		}
	}

	void ExecuteEvent(DialogueEvent evt)
	{
		if (evt == null) return;

		switch (evt.eventType)
		{
			case DialogueEventType.None:
				break;

			case DialogueEventType.CloseDialogue:
				lockTarget?.UnlockControl();
				transform.localScale = Vector3.zero;
				break;

			case DialogueEventType.LoadScene:
				SceneManager.LoadScene(evt.parameter);
				break;

			case DialogueEventType.StartDialogue:
				DialogueData nextData = Resources.Load<DialogueData>(evt.parameter);
				StartDialogue(Int32.Parse(evt.parameter));
				break;

			case DialogueEventType.AlertUnexpectedSituation:
				StartCoroutine(AlertUnexpected(evt.parameter));
				break;
		}
	}

	private IEnumerator AlertUnexpected(string situationName)
	{

		yield return null;

	}



	void NextLine()
	{
		ExecuteEvent(currentData.lines[currentIndex].lineEvent);
		currentIndex++;
		if (currentIndex < currentData.lines.Length)
		{ShowLine();}
		else
		{EndDialogue();}
	}

	void OnChoiceSelected(DialogueChoice choice)
	{
		ExecuteEvent(choice.choiceEvent);
		currentIndex = choice.nextDialogueIndex;
		ShowLine();
	}

	void EndDialogue()
	{
		// 대화 끝났을 때도 이벤트 실행 가능
		ExecuteEvent(new DialogueEvent { eventType = DialogueEventType.CloseDialogue });
	}

	void ClearChoices()
	{
		if (choiceContainer == null) return;

		foreach (Transform child in choiceContainer)
		{Destroy(child.gameObject);}
	}

	public void SetLockTarget(ILockable target)
	{
		lockTarget = target;}

}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
	public DialogueNode[] lines;
}