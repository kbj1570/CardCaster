using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNodeSO", menuName ="Scriptable Object/DialogueNodeSO")]
public class DialogueNodeSO : ScriptableObject
{

    [SerializeField] DialogueNodeSO nextNode_First;
    [SerializeField] DialogueNodeSO nextNode_Second;
    [SerializeField] DialogueNodeSO nextNode_Third;
    [SerializeField] Sprite characterSprite_Left;
    [SerializeField] Sprite characterSprite_Middle;
    [SerializeField] Sprite characterSprite_Right;
    [SerializeField] string text_1;
    [SerializeField] string text_2;
    [SerializeField] string text_3;

    [SerializeField] string selectionText_First;
    [SerializeField] string selectionText_Second;
    [SerializeField] string selectionText_Third;
    [SerializeField] string speakerName;
    [SerializeField] bool switchSequence;
    public bool hasSelection;
    [SerializeField] int sequenceValue;
    public DialogueOwner dialogueOwner;


    public string GetName(){return speakerName;}
    public string GetText_1(){return text_1;}
    public string GetText_2(){return text_2;}
    public string GetText_3(){return text_3;}
    public string GetSelectionText_1(){return selectionText_First;}
    public string GetSelectionText_2(){return selectionText_Second;}
    public string GetSelectionText_3(){return selectionText_Third;}
    public DialogueNodeSO GetNextNode(int value)
    {
        switch(value)
        {
            case 1:
            return nextNode_First;

            case 2:
            return nextNode_Second;

            case 3:
            return nextNode_Third;

            default:
            return nextNode_First;
        }
    }

    public DialogueNodeSO GetNextNode_First()
    {return nextNode_First;}
    
    public DialogueNodeSO GetNextNode_Second()
    {return nextNode_Second;}

    public DialogueNodeSO GetNextNode_Third()
    {return nextNode_Third;}


    public bool GetSwitchSequence(){return switchSequence;}
    public int GetSequenceValue(){return sequenceValue;}
    public DialogueOwner GetDialogueOwner(){return dialogueOwner;}
    public bool GetHasSelection(){return hasSelection;}

}

public enum DialogueOwner{ESideKick, ESoulCollector, EMarry}