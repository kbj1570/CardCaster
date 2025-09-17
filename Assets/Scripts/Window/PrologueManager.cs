using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrologueManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    void Start()
    {

        PlayerData.saveData = DataController.Inst.LoadData();
        dialogueManager.StartDialogue(0);
    }

    public void CreatePrologueDungeon()
    {
        DungeonData.dungeon = new RedForest();

        List<ERoomType> tileType = new()
        {
            ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EStair,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.EWall,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.None,ERoomType.EWall,
            ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall,ERoomType.EWall
        };
        DungeonData.map = new()
        {
            null, null,null, null,null, null,null, null,null, null,
            null, null,null, null,null, null,null, null,null, null,
            null, null,null, null,null, null,null, null,null, null,
            null, null,null, null,null, null,null, null,null, null,
            null, null,null, null,null, null,null, null,null, null,
            null, null,null, null,null, null,null, null,null, null,
            null, null,null, null,null, null,null, null,null, null,
            null, null,null, null,null, null,null, null,null, null,
            null, null,null, null,null, null,null, null,null, null,
            null, null,null, null,null, null,null, null,null, null
        };

        int[] numList = {1,};

        for (int i = 0; i < tileType.Count; ++i)
        {
            
            DungeonData.map[i] = new Node();
            if (numList.Contains(i))
            {DungeonData.map[i].SetRoomType(tileType[i]);}
            else
            {DungeonData.map[i].SetRoomType(tileType[i]);}
        }
	}
}
