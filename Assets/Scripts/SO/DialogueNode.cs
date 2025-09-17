using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueEvent
{
	public DialogueEventType eventType;
	[TextArea] public string parameter;
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
	public Sprite portrait;
	public Sprite background;
	public string speaker;
	[TextArea] public string text;
	public DialogueChoice[] choices;
	public DialogueEvent lineEvent;
}

public enum DialogueEventType
{
	None,
	CloseDialogue,
	LoadScene,
	StartDialogue,
	GetItem,
	GetGold,
	AlertUnexpectedSituation,
	FadeInOut,
	FadeIn,
	FadeOut,
	AddValue,
	SetPortrait,
	HidePortrait,
	FadeInSituationImage,
	ShowDocument,
	SetBackGround,
	PlaySound
}