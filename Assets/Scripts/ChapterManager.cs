using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterManager : MonoBehaviour
{

    public GameObject imageObject;
    public DialogueManager dialogueManager;
    public int chapterNum;
    void Start()
    {
        // PlayerData.saveData = DataController.Inst.LoadData();
        PlayerData.saveData = DataController.Inst.LoadData();
        ReadyChapter();
    }

    public void ReadyChapter()
    {

        switch (chapterNum)
        {
            case 0:
                {
                    imageObject.SetActive(false);
                    DungeonData.dungeon = new RedForest();
                    dialogueManager.StartDialogue(0);
                    break;
                }
            case 1:
                {
                    imageObject.SetActive(false);
                    DungeonData.dungeon = new PrologueCorrupted();
                    dialogueManager.StartDialogue(8);
                    break;
                }
            case 2:
                {
                    DungeonData.dungeon = new PrologueCorruptedDummy();
                    SceneManager.LoadScene("Dungeon");
                    break;
                }
        }
    }
}
