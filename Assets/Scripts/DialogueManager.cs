using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class DialogueManager : MonoBehaviour
{
	public TMP_Text speakerText;
	public TMP_Text dialogueText;
	public GameObject choiceButtonPrefab;
	public Transform choiceContainer;

	public Transform alertPosition;
	public GameObject textBox;

	private DialogueData currentData;
	public List<DialogueData> dialogueList;
	int currentIndex = 0;
    private ILockable lockTarget;

	public GameObject alertObject;
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
			Debug.LogError("Àß¸øµÈ Dialogue ID: " + dialogueId);
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
				StartCoroutine(NextLine());
			}
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
				SceneManager.LoadScene(evt.parameter);
				break;

			case DialogueEventType.StartDialogue:
				DialogueData nextData = Resources.Load<DialogueData>(evt.parameter);
				StartDialogue(Int32.Parse(evt.parameter));
				break;

			case DialogueEventType.AlertUnexpectedSituation:
				yield return StartCoroutine(AlertUnexpected(evt.parameter));
				break;
		}
	}

	private IEnumerator AlertUnexpected(string situationName)
	{
		textBox.SetActive(false);
		GameObject onMessage = Instantiate(alertObject, this.transform);
		onMessage.transform.position = alertPosition.position;
		onMessage.GetComponent<AlertMessage>().SetText(situationName);
		StartCoroutine(onMessage.GetComponent<AlertMessage>().FadeInOut());
		yield return new WaitForSeconds(2f);
		textBox.SetActive(true);


	}



	IEnumerator NextLine()
	{
		yield return StartCoroutine(ExecuteEvent(currentData.lines[currentIndex].lineEvent));
		currentIndex++;
		if (currentIndex < currentData.lines.Length)
		{ShowLine();}
		else
		{EndDialogue();}
	}

	void OnChoiceSelected(DialogueChoice choice)
	{
		StartCoroutine(ExecuteEvent(choice.choiceEvent));
		currentIndex = choice.nextDialogueIndex;
		ShowLine();
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

