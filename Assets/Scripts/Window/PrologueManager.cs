using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrologueManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    void Start()
    {

        PlayerData.saveData = DataController.Inst.LoadData();
        CreatePrologueDungeon();
        dialogueManager.StartDialogue(0);
    }

    public void CreatePrologueDungeon()
    {
        DungeonData.dungeon = new RedForest();
	}
}
