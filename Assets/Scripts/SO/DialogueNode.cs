using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueEvent
{
	public DialogueEventType eventType;
	public string parameter;
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
	public Sprite mugShot;
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
	GiveItem,
	AlertUnexpectedSituation,
	FadeInOut,
	AddValue,
	SetMugShot,
	HideMugShot,
	ShowMugShot,
	FadeInSituationImage
}