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

	public DialogueData dialogueData;
	private DialogueNode[] dialogueNodes;
	int currentIndex = 0;

	void Start()
	{
		dialogueNodes = dialogueData.lines;
		ShowLine();
	}

	void ShowLine()
	{
		ClearChoices();

		DialogueNode line = dialogueNodes[currentIndex];
		speakerText.text = line.speaker;
		dialogueText.text = line.text;

		// 선택지가 있으면 버튼 생성
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
			DialogueNode line = dialogueNodes[currentIndex];
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
				// 대화창만 닫기
				gameObject.SetActive(false);
				break;

			case DialogueEventType.LoadScene:
				SceneManager.LoadScene(evt.parameter);
				break;

			case DialogueEventType.StartDialogue:
				// 다른 DialogueData 불러오기
				DialogueData nextData = Resources.Load<DialogueData>(evt.parameter);
				if (nextData != null)
				{
					dialogueNodes = nextData.lines;
					currentIndex = 0;
					ShowLine();
				}
				break;
		}
	}



	void NextLine()
	{
		// 현재 라인의 이벤트 실행
		ExecuteEvent(dialogueNodes[currentIndex].lineEvent);

		currentIndex++;
		if (currentIndex < dialogueNodes.Length)
		{
			ShowLine();
		}
		else
		{
			EndDialogue();
		}
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
		{
			Destroy(child.gameObject);
		}
	}
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
	public DialogueNode[] lines;
}