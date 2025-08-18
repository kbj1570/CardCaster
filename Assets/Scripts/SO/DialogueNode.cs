using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode
{

    DialogueNode nextNode_First;
    DialogueNode nextNode_Second;
    DialogueNode nextNode_Third;
    string text_1;
    string text_2;
    string text_3;

    string selectionText_First;
    string selectionText_Second;
    string selectionText_Third;
    string speakerName;
    bool switchSequence;
    public bool hasSelection;
    int sequenceValue;
    ItemData requirement;

    public string GetName(){return speakerName;}
    public string GetText_1(){return text_1;}
    public string GetText_2(){return text_2;}
    public string GetText_3(){return text_3;}
    public string GetSelectionText_1(){return selectionText_First;}
    public string GetSelectionText_2(){return selectionText_Second;}
    public string GetSelectionText_3(){return selectionText_Third;}
    public DialogueNode GetNextNode(int value)
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

    public DialogueNode GetNextNode_First()
    {return nextNode_First;}
    
    public DialogueNode GetNextNode_Second()
    {return nextNode_Second;}

    public DialogueNode GetNextNode_Third()
    {return nextNode_Third;}


    public bool GetSwitchSequence(){return switchSequence;}
    public int GetSequenceValue(){return sequenceValue;}
    public bool GetHasSelection(){return hasSelection;}

}
