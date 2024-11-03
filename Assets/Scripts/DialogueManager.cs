using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{

    public int sidekickDialogueSequence;
    public int soulCollectorDialogueSequence;

    public int dialogueSequence;
    public Image characterSprite_Left;
    public Image characterSprite_Middle;
    public Image characterSprite_Right;
    private int switchNumber;   


    [SerializeField] GameObject dialogueWindow;
    [SerializeField] TMP_Text nameField;
    [SerializeField] TMP_Text textField_1;
    [SerializeField] TMP_Text textField_2;
    [SerializeField] TMP_Text textField_3;
    [SerializeField] TMP_Text selectionTextField_1;
    [SerializeField] TMP_Text selectionTextField_2;
    [SerializeField] TMP_Text selectionTextField_3;



    public DialogueNodeSO currentDialogue;
    public List<DialogueNodeSO> dialogueNodeList;
    public List<DialogueNodeSO> merchantDialogueNodeList;
    public Button selectButton_1;
    public Button selectButton_2;
    public Button selectButton_3;
    public Button frameButton;



    // Start is called before the first frame update
    void Start()
    {
        dialogueWindow.SetActive(false);
        // currentDialogue = dialogueNodeList[dialogueSequence];
    }

    public void OpenDialogue(){dialogueWindow.SetActive(true);}
    public void CloseDialogue(){dialogueWindow.SetActive(false);}

    public void ShowDialogue()
    {
        selectButton_1.gameObject.SetActive(false);
        selectButton_2.gameObject.SetActive(false);
        selectButton_3.gameObject.SetActive(false);

        if(currentDialogue.GetHasSelection())
        {
            frameButton.enabled = false;

            if(currentDialogue.GetNextNode_First() != null)
            {selectButton_1.gameObject.SetActive(true);}

            if(currentDialogue.GetNextNode_Second() != null)
            {selectButton_2.gameObject.SetActive(true);}

            if(currentDialogue.GetNextNode_Third() != null)
            {selectButton_3.gameObject.SetActive(true);}

            selectionTextField_1.text = currentDialogue.GetSelectionText_1();
            selectionTextField_2.text = currentDialogue.GetSelectionText_2();
            selectionTextField_3.text = currentDialogue.GetSelectionText_3();
        } else
        {
            frameButton.enabled = true;
        }
        nameField.text = currentDialogue.GetName();
        textField_1.text = currentDialogue.GetText_1();
        textField_2.text = currentDialogue.GetText_2();
        textField_3.text = currentDialogue.GetText_3();

        if(currentDialogue.GetSwitchSequence())
        {
            switch(currentDialogue.GetDialogueOwner())
            {
                case DialogueOwner.ESideKick:
                sidekickDialogueSequence = currentDialogue.GetSequenceValue();
                break;

                case DialogueOwner.ESoulCollector:
                soulCollectorDialogueSequence = currentDialogue.GetSequenceValue();
                break;
            }
        }
    }

    public void LoadNextDialogue(){currentDialogue = currentDialogue.GetNextNode(switchNumber);}
    public void SetSwitchNumber(int value){switchNumber = value;}
}
