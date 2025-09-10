using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueEvent
{
	public DialogueEventType eventType;
	public string parameter; // 씬 이름, 아이템 ID, 다음 대화 ID 등
}

[System.Serializable]
public class DialogueChoice
{
	public string choiceText;
	public int nextDialogueIndex;
	public DialogueEvent choiceEvent;
}

[System.Serializable]
public class DialogueNode
{
	public string speaker;
	public string text;
	public DialogueChoice[] choices;
	public DialogueEvent lineEvent;
}

public enum DialogueEventType
{
	None,
	CloseDialogue,
	LoadScene,
	StartDialogue,
	GiveItem
}