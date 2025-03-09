using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using System;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    Dungeon dungeon;
    public Camera camera;
    public Node startNode;
    int width;
    int height;
    int maxGold;
    int dungeonEndFloor;
    List<int> safeFloorList;
    List<Item> itemDatabase;
    public int floor;
    int floorSize;
    string dungeonName;
    List<Node> map;
    List<GameObject> nodeMap;
    List<int> nodeNumList;
    List<string> messageList;
    Dictionary<Item, int> itemList;
    Dictionary<Enemy, int> enemyList;
    public GameObject roomNodePrefab;
    public GameObject itemNodePrefab;
    public GameObject encounterNodePrefab;
    public GameObject monsterNodePrefab;
    public GameObject stairNodePrefab;
    public GameObject mapObject;
    public GameObject buttonPrefab;
    public GameObject player;
    public GameObject stairAlert;
    public GameObject wayPointWindow;

    public List<GameObject> itemPrefabList;

    public List<Transform> itemLocation;

    private Dictionary<Item, int> currentItemList;
    private Dictionary<Item, int> myItemList;

    public GameObject popUpMessageWindow;
    public GameObject popUpMessage;

    public GameObject nextButton;
    public GameObject backButton;

    public List<GameObject> buttonList;

    public TMP_Text dungeonNameText;
    public TMP_Text floorText;
    public TMP_Text encounterName;
    public TMP_Text encounterDescription;

    public TMP_Text healthText;
    public TMP_Text goldText;
    public TMP_Text textbox;

    private Encounter currentEncounter;

    public Item selectedItem;

    private int currentPlayerLocation;

    private int goldMultiple;
    private bool isIgnorable;
    public static DungeonManager Inst{get; private set;}

    private int selectionValue;
    int currentPage;
    void Awake() => Inst = this;

    List<GameObject> itemObjectList;


    // public float moveSpeed = 2f; // 이동 속도
    // private Vector2Int currentPos; // 현재 좌표
    // private Vector2Int targetPos; // 목표 좌표
    // private Queue<Vector2Int> pathQueue = new Queue<Vector2Int>(); // 이동 경로 큐

    // public Dictionary<Vector2Int, GameObject> gridMap; // 생성된 칸을 저장하는 딕셔너리

    private void Start()
    {
        
        DungeonSetUp();
        // LoadEncounter();
        // ShowEncounter();
        CreateFloor();
        camera.transform.position = player.transform.position;
        camera.transform.position += new Vector3(0,0,-1);

        itemObjectList = new();
        messageList = new();

        myItemList = new();

        currentItemList = new();
        itemDatabase = DataController.Inst.LoadItemDatabase();
        LoadItemList();

        UpdateItemPage();

        currentPage = 0;

        // currentPos = new Vector2Int(0, 0); // 초기 위치
        // targetPos = currentPos;
        // transform.position = new Vector3(currentPos.x, currentPos.y, 0);
    }
    public void SetDungeon(Dungeon dungeon)
    {this.dungeon = dungeon;}

    public void SetSelectedItem(int itemNum)
    {selectedItem = itemDatabase[itemNum];}

    public void LoadItemList()
    {
        foreach(KeyValuePair<string, int> value in DataController.Inst.LoadItemList())
        {myItemList.Add(itemDatabase[Convert.ToInt32(value.Key)], value.Value);}
    }

     public void UpdateItemPage()
    {
        int count = 0;
        int pageLimit = myItemList.Count / 8;
        int remainder = myItemList.Count % 8;

        currentItemList.Clear();
        foreach(GameObject gameObject in itemObjectList)
        {Destroy(gameObject);}

        List<Item> cardList = new List<Item>(myItemList.Keys);

        if(currentPage != pageLimit)
        {remainder = 6;}

        for(int i = 0; i < remainder; ++i)
        {currentItemList.Add(cardList[(currentPage * 6) + i], myItemList[cardList[(currentPage * 6) + i]]);}

        foreach(KeyValuePair<Item, int> item in currentItemList)
        {
            int x = Convert.ToInt32(item.Key.GetNum());
            GameObject cardObject = Instantiate(itemPrefabList[x],
            new Vector3(0,0,0) , Utils.QI);
            cardObject.transform.SetParent(itemLocation[count].transform);
            cardObject.transform.localScale = new Vector3(0.55f,0.55f,0.55f);
            cardObject.transform.localPosition = new Vector3(0,0,0);
            
            itemObjectList.Add(cardObject);

            bool locked = false;

            if(item.Value == 0)
            {locked = true;}

            // if(myDeckList.ContainsKey(item.Key))
            // {
            //     if(myDeckList[item.Key]  == item.Value)
            //     {locked = true;}

            //     if(myDeckList[item.Key]  == 3)
            //     {locked = true;}
            // }

            

            // GameObject cardFrameObject = Instantiate(cardFrame,new Vector3(0,0,0) , Utils.QI);
            // cardFrameObject.transform.SetParent(cardLocation[count].transform);
            // cardFrameObject.transform.localPosition = new Vector3(0,0,0);
            // cardFrameObject.GetComponent<CardFrame>().
            // SetCardData(item.Key, item.Value, count, locked);

            // dummyCardObjectList.Add(cardFrameObject);
            count++;

            
        }
        if(currentPage == 0)
        backButton.SetActive(false);
        else
        backButton.SetActive(true);

        if(currentPage == pageLimit)
        nextButton.SetActive(false);
        else
        nextButton.SetActive(true);

        // pageNumber.text = (currentPage + 1) + " / " + (pageLimit + 1);        
    }


    private void DungeonSetUp()
    {
        dungeon = new Graveyard();
        dungeonName = dungeon.GetDungeonName();
        width = dungeon.GetDungeonWidth();
        height = dungeon.GetDungeonHeight();
        floorSize = dungeon.GetDungeonFloorSize();
        maxGold = dungeon.GetMaxGold();
        dungeonEndFloor = dungeon.GetDungeonEndFloor();
        safeFloorList = dungeon.GetSafeFloorList();
        itemList = dungeon.GetItemList();
    }


    // public void OnTileClicked(Vector2Int clickedPos)
    // {
    //     if (!gridMap.ContainsKey(clickedPos)) return; // 이동할 칸이 없으면 무시

    //     // 이동 경로를 계산 (직선 이동)
    //     pathQueue.Clear();
    //     Vector2Int pos = currentPos;

    //     while (pos != clickedPos)
    //     {
    //         if (pos.x < clickedPos.x) pos.x++;
    //         else if (pos.x > clickedPos.x) pos.x--;
    //         else if (pos.y < clickedPos.y) pos.y++;
    //         else if (pos.y > clickedPos.y) pos.y--;

    //         pathQueue.Enqueue(pos);
    //     }
    // }

    // private void MoveToNextTile()
    // {
    //     if (pathQueue.Count == 0) return;

    //     Vector2Int nextPos = pathQueue.Dequeue();
    //     targetPos = nextPos;

    //     StartCoroutine(MoveSmoothly(nextPos));
    // }

    // IEnumerator MoveSmoothly(Vector2Int nextPos)
    // {
    //     Vector3 start = transform.position;
    //     Vector3 end = new Vector3(nextPos.x, nextPos.y, 0);
    //     float elapsedTime = 0f;

    //     while (elapsedTime < 1f / moveSpeed)
    //     {
    //         transform.position = Vector3.Lerp(start, end, elapsedTime * moveSpeed);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     transform.position = end;
    //     currentPos = nextPos;
    // }

    void AlertPopUpMessage(string message)
    {
        GameObject onMessage = Instantiate(popUpMessage, popUpMessageWindow.transform);
        onMessage.GetComponent<PopUpMessage>().SetText(message);
    }
    public void ShowMessage()
    {
        if(messageList.Count == 0)
        return;
    }


    void Update()
    {
        ShowMessage();

        // if (pathQueue.Count > 0)
        // {
        //     MoveToNextTile();
        // }

        if(Input.GetKeyDown(KeyCode.W))
        {
            if(CheckOutOfIndex(currentPlayerLocation - width))
            {
                if(nodeMap[currentPlayerLocation - width] != null)
                {
                    currentPlayerLocation -= width;
                    MovePlayer(currentPlayerLocation);
                }
            }
            camera.transform.position = player.transform.position;
            camera.transform.position += new Vector3(0,0,-1);
        }

        if(Input.GetKeyDown(KeyCode.A))
        {

            if(currentPlayerLocation % width == 0)
            return;


            if(CheckOutOfIndex(currentPlayerLocation - 1))
            {
                if(nodeMap[currentPlayerLocation - 1] != null)
                {
                    currentPlayerLocation -= 1;
                    MovePlayer(currentPlayerLocation);
                }
                camera.transform.position = player.transform.position;
                camera.transform.position += new Vector3(0,0,-1);
            }
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            if(CheckOutOfIndex(currentPlayerLocation + width))
            {
                if(nodeMap[currentPlayerLocation + width] != null)
                {
                    currentPlayerLocation += width;
                    MovePlayer(currentPlayerLocation);
                }
                camera.transform.position = player.transform.position;
                camera.transform.position += new Vector3(0,0,-1);
            }
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            if(currentPlayerLocation % width == width - 1)
            return;

            if(CheckOutOfIndex(currentPlayerLocation + 1))
            {
                if(nodeMap[currentPlayerLocation + 1] != null)
                {
                    currentPlayerLocation += 1;
                    MovePlayer(currentPlayerLocation);
                }
                camera.transform.position = player.transform.position;
                camera.transform.position += new Vector3(0,0,-1);
            }
        }
    }

    
    private void SetNodeRoom()
    {
        // int x = Random.Range(0, nodeNumList.Count);
        // currentPlayerLocation = nodeNumList[x];
        
        // nodeNumList.Remove(nodeNumList[x]);

        // x = Random.Range(0, nodeNumList.Count);
        // map[nodeNumList[x]].SetRoomType(ERoomType.EStair);
        // nodeNumList.Remove(nodeNumList[x]);

        // x = Random.Range(0, nodeNumList.Count);
        // map[nodeNumList[x]].SetRoomType(ERoomType.EEncount);
        // nodeNumList.Remove(nodeNumList[x]);

        // x = Random.Range(0, nodeNumList.Count);
        // map[nodeNumList[x]].SetRoomType(ERoomType.EMonster);
        // nodeNumList.Remove(nodeNumList[x]);


        int x = Random.Range(0, nodeNumList.Count);
        currentPlayerLocation = nodeNumList[x];

        x = Random.Range(0, nodeNumList.Count);
        map[nodeNumList[x]].SetRoomType(ERoomType.EEncount);

        x = Random.Range(0, nodeNumList.Count);
        map[nodeNumList[x]].SetRoomType(ERoomType.EMonster);
        SetEnemyInNode(map[nodeNumList[x]]);

        x = Random.Range(0, nodeNumList.Count);
        map[nodeNumList[x]].SetRoomType(ERoomType.EMonster);
        SetEnemyInNode(map[nodeNumList[x]]);

        x = Random.Range(0, nodeNumList.Count);
        map[nodeNumList[x]].SetRoomType(ERoomType.EStair);

        // 중복되지 않는 랜덤숫자 생성

        for(int i = 0; i < 5; ++i)
        {
            x = Random.Range(0, nodeNumList.Count);

            if(Random.Range(0, 2) == 0)
            {
                map[nodeNumList[x]].SetRoomType(ERoomType.EItem);
                map[nodeNumList[x]].SetItem(ReturnDungeonItem(), Random.Range(0, 2));

            }else
            {
                map[nodeNumList[x]].SetRoomType(ERoomType.EGold);
                map[nodeNumList[x]].SetGold(Random.Range(1, maxGold + 1));
            }
            nodeNumList.Remove(nodeNumList[x]);
        }
    }

    private Item ReturnDungeonItem()
    {
        if(itemList != null)
        {   
            int count = 0;
            Dictionary<Item, int> rewardRoullet = new();

            foreach(KeyValuePair<Item, int> reward in itemList)
            {
                count += reward.Value;
                rewardRoullet.Add(reward.Key, count);
            }

            int randomNum = Random.Range(0, count + 1);

            foreach(KeyValuePair<Item, int> reward in rewardRoullet)
            {
                if(randomNum <= reward.Value)
                {
                    randomNum = Random.Range(1, 3);
                    return reward.Key;
                }
            }
        }

        return null;
    }

    public void SetEnemyInNode(Node node)
    {
        node.SetEnemy(new UnknownMonster());
    }

    public void CreateFloor()
    {
        nodeNumList = new List<int>();
        while(nodeNumList.Count < 20) // 생성된 층의 크기가 10보다 작으면 다시 만듬
        {
            map = new();
            for(int i = 0; i < floorSize; ++i)
            {map.Add(null);}
            CreateFirstRoom();
            CreateNodeNumList();
        }

        floorText.text = floor.ToString() + "F";
        AddLoopCorridor();
        CreateNodeNumList();
        SetNodeRoom();
        InstantiateNode();
        
        MovePlayer(currentPlayerLocation);
    }

    public void DestroyFloor()
    {
        foreach(GameObject node in nodeMap)
        {Destroy(node);}
    }

    public void AddLoopCorridor()
    {

        int random = Random.Range(0,3);
        int first = nodeNumList[random];
        int last = nodeNumList[nodeNumList.Count - 1];

        int x = (last / width) - (first / width);
        int z = first;

        for(int p = 0; p < x; ++p)
        {
            z = z + width;
            if(map[z] == null)
            {map[z] = new();}

            if(!nodeNumList.Contains(z))
            {nodeNumList.Add(z);}
        }

        for(int q = z; q < last; ++q)
        {
            z = z + 1;
            if(map[z] == null)
            {map[z] = new();}

            if(!nodeNumList.Contains(z))
            {nodeNumList.Add(z);}
        }
    }


    public void UpdateNodeBlocked()
    {
        for(int i = 0; i < nodeNumList.Count; ++i)
        {
            nodeMap[nodeNumList[i]].GetComponent<RoomNode>().SetBlocked(
                !nodeNumList.Contains(nodeNumList[i] - width),
                !nodeNumList.Contains(nodeNumList[i] + width),
                !nodeNumList.Contains(nodeNumList[i] - 1),
                !nodeNumList.Contains(nodeNumList[i] + 1));
            
            Debug.Log(nodeNumList[i]);
        }
    }

    private void InstantiateNode()
    {
        nodeMap = new();

        for(int i = 0; i < floorSize; ++i)
        {nodeMap.Add(null);}

        for(int i = 0; i < map.Count; ++i)
        {
            if(map[i] != null)
            {
                GameObject prefab = null;
                switch(map[i].GetRoomType())
                {
                    case ERoomType.EStair:
                    prefab = stairNodePrefab;
                    break;

                    case ERoomType.EEncount:
                    prefab = encounterNodePrefab;
                    break;

                    case ERoomType.EGold:
                    prefab = itemNodePrefab;
                    break;

                    case ERoomType.EItem:
                    prefab = itemNodePrefab;
                    break;

                    case ERoomType.EMonster:
                    prefab = monsterNodePrefab;
                    break;

                    case ERoomType.None:
                    prefab = roomNodePrefab;
                    break;

                }

                GameObject gameObject = Instantiate(prefab,new Vector3(), Utils.QI);
                gameObject.transform.SetParent(mapObject.transform);
                gameObject.transform.position = mapObject.transform.position;
                gameObject.GetComponent<RoomNode>().SetNodeNum(i);
                gameObject.GetComponent<RoomNode>().SetRoomType(map[i].GetRoomType());
                gameObject.transform.position = mapObject.transform.position + CalculateNodePosition(i);
                gameObject.SetActive(false);

                 
                nodeMap.Insert(i, gameObject);
            }
        }
    }

    // private void SearchNodeRoute(int roomNum, int end)
    // {
    //     if(CheckOutOfIndex(roomNum - 1))
    //     if(map[roomNum - 1] == null)
    //     if(FindCloseNodes(new List<int>(), roomNum - 1, -1, end) == null)
    //     {}
    // }

    private void CreateFirstRoom()
    {
        int roomNum = floorSize / 2;
        map[roomNum] = new Node();
        map[roomNum].SetRoomType(ERoomType.None);

        CreateRoomNode(roomNum + 1);
        CreateRoomNode(roomNum - 1);
        CreateRoomNode(roomNum + width);
        CreateRoomNode(roomNum - width);
    }

    // private List<int> FindCloseNodes(List<int> list, int value, int direction, int target)
    // {
    //     if(!CheckOutOfIndex(value))
    //     {return null;}

    //     if(map[value] == null)
    //     {return null;}

    //     if(value)

    //     if(nodeNumList.Contains(value))
    //     {
    //         Debug.Log(value);
    //         list.Add(value);
    //     }
    
    //     return FindCloseNodes(list, value + direction, direction, target);
    // }

    private void CreateRoomNode(int roomNum)
    {
        if(!CheckOutOfIndex(roomNum))
        {return;}

        if(map[roomNum] != null)
        {return;}

        // if(roomNum / width == 0)
        // {return;}

        if(TrueOrFalse())
        {return;}

        if(CountNeighbourhood(roomNum) >= 2)
        {return;}

        map[roomNum] = new Node();

    
        CreateRoomNode(roomNum + 1);
        CreateRoomNode(roomNum - 1);
        CreateRoomNode(roomNum + width);
        CreateRoomNode(roomNum - width);
    }

    private Vector3 CalculateNodePosition(int roomNum)
    {
        int x = ((roomNum  % width) - 9) * 2;
        int y = -(((roomNum / width) - 8) * 2);

        return new Vector3(x, y, 0);
    }

    private int CountNeighbourhood(int roomNum)
    {
        int count = 0;

        if(CheckOutOfIndex(roomNum - 1))
        {
            if(map[roomNum - 1] != null)
            {count++;}
        }

        if(CheckOutOfIndex(roomNum + 1))
        {
            if(map[roomNum + 1] != null)
            {count++;}
        }

        if(CheckOutOfIndex(roomNum + width))
        {
            if(map[roomNum + width] != null)
            {count++;}
        }

        if(CheckOutOfIndex(roomNum - width))
        {
            if(map[roomNum - width] != null)
            {count++;}
        }

        return count;

    }

    private bool CheckOutOfIndex(int roomNum)
    {
        if(roomNum < 0)
        {return false;}

        if(roomNum > floorSize - 1)
        {return false;}

        return true;
    }

    private void CreateNodeNumList()
    {
        nodeNumList = new List<int>();
        for(int i = 0; i < map.Count; ++i)
        {
            if(!CheckCorridor(i))
            {nodeNumList.Add(i);}
        }
    }

    private bool CheckCorridor(int value)
    {

        if(!CheckOutOfIndex(value))
        {return true;}

        if(map[value] == null)
        {return true;}

        if(CountNeighbourhood(value) != 2)
        {return false;}

        if(CheckOutOfIndex(value - 1) && CheckOutOfIndex(value + 1))
        {
            if(map[value + 1] != null && map[value - 1] != null)
            {return true;}
        }

        if(CheckOutOfIndex(value - width) && CheckOutOfIndex(value + width))
        {
            if(map[value + width] != null && map[value - width] != null)
            {return true;}
        }
        
        return false;
    }

    public void GoToNextFloor()
    {
        floor++;

        if(safeFloorList.Contains(floor))
        {
            wayPointWindow.GetComponent<Window>().OnOff();
            DestroyFloor();
        }
        else if(dungeonEndFloor == floor)
        {Debug.Log("던전을 클리어 했습니다");}
        else
        {
            DestroyFloor();
            CreateFloor();
        }
    }

    public void UseItem()
    {
        Item item = new();

        switch(item.GetNum())
        {
            case "0": // 빨간포션
            PlayerManager.Inst.GainHealth(2);
            break;

            case "1": // 거대한포션
            PlayerManager.Inst.GainHealth(6);
            break;

            case "2": // 황금주사위
            
            break;

            case "3": // 부숴진나침반
            
            break;

            case "4": // 불길한향로
            
            break;
        }
    }

    public void IncreaseSelectionValue()
    {selectionValue++;}
    public void DecreaseSelectionValue()
    {selectionValue--;}
    public int GetSelectionValue()
    {return selectionValue;}
    

    public void MovePlayer(int roomNum)
    {
        player.transform.position = CalculateNodePosition(roomNum) + mapObject.transform.position;
        nodeMap[roomNum].SetActive(true);
        nodeMap[roomNum].GetComponent<RoomNode>().SetVisited();

        if(CheckOutOfIndex(roomNum + 1) && roomNum % width != width - 1)
        {
            if(nodeMap[roomNum + 1] != null)
            {
                if(!nodeMap[roomNum + 1].active)
                {
                    nodeMap[roomNum + 1].SetActive(true);
                    StartCoroutine(nodeMap[roomNum + 1].GetComponent<RoomNode>().FadeOut());
                }
                
            }
        }

        if(CheckOutOfIndex(roomNum + width))
        {
            if(nodeMap[roomNum + width] != null)
            {
                if(!nodeMap[roomNum + width].active)
                {
                    nodeMap[roomNum + width].SetActive(true);
                    StartCoroutine(nodeMap[roomNum + width].GetComponent<RoomNode>().FadeOut());
                }
            }
        }

        if(CheckOutOfIndex(roomNum - 1) && roomNum % width != 0)
        {
            if(nodeMap[roomNum - 1] != null)
            {
                if(!nodeMap[roomNum - 1].active)
                {
                    nodeMap[roomNum - 1].SetActive(true);
                    StartCoroutine(nodeMap[roomNum - 1].GetComponent<RoomNode>().FadeOut());
                }
            }

            
        }

        if(CheckOutOfIndex(roomNum - width))
        {
            if(nodeMap[roomNum - width] != null)
            {
                if(!nodeMap[roomNum - width].active)
                {
                    nodeMap[roomNum - width].SetActive(true);
                    StartCoroutine(nodeMap[roomNum - width].GetComponent<RoomNode>().FadeOut());
                }
            }
        }

        if(map[roomNum].GetRoomType() == ERoomType.EStair)
        {OpenStairAlert();}
        else if(map[roomNum].GetRoomType() == ERoomType.EMonster)
        {
            map[roomNum].SetRoomType(ERoomType.None);
        }
        else if(map[roomNum].GetRoomType() == ERoomType.EEncount)
        {
            map[roomNum].SetRoomType(ERoomType.None);
        }
        else if(map[roomNum].GetRoomType() == ERoomType.EGold)
        {
            GainGold(map[roomNum]);
            map[roomNum].SetRoomType(ERoomType.None);
        }
        else if(map[roomNum].GetRoomType() == ERoomType.EItem)
        {
            GainItem(map[roomNum]);
            map[roomNum].SetRoomType(ERoomType.None);
        }
    }

    

    private void GainGold(Node node)
    {
        AlertPopUpMessage(node.GetGold().ToString() + " " +"골드 획득");
    }

    private void GainItem(Node node)
    {
        AlertPopUpMessage(node.GetItem().GetName() + " " +" 획득");
    }

    public void OpenStairAlert()
    {stairAlert.GetComponent<Window>().OnOff();}



    // public void ShowEncounter()
    // {
    //     selectionValue = 0;
    //     buttonList[0].SetActive(true);
    //     buttonList[1].SetActive(false);
    //     buttonList[2].SetActive(false);
    //     buttonList[3].SetActive(true);

    //     encounterName.text = currentEncounter.GetEncounterName();
    //     encounterDescription.text = currentEncounter.GetEncounterDescription();

    //     List<string> select = currentEncounter.GetSelect();

    //     buttonList[0].GetComponent<MyButton>().SetText(select[0]);

    //     switch(select.Count)
    //     {
    //         case 2:
    //         buttonList[1].SetActive(true);
    //         buttonList[1].GetComponent<MyButton>().SetText(select[1]);
    //         break;

    //         case 3:
    //         buttonList[1].SetActive(true);
    //         buttonList[1].GetComponent<MyButton>().SetText(select[1]);

    //         buttonList[2].SetActive(true);
    //         buttonList[2].GetComponent<MyButton>().SetText(select[2]);
    //         break;
    //     }
    // }

    // public void ShowResult(int value)
    // {
    //     buttonList[0].SetActive(false);
    //     buttonList[1].SetActive(false);
    //     buttonList[2].SetActive(false);
    //     buttonList[3].SetActive(false);

    //     encounterDescription.text = currentEncounter.GetResult()[value];

    //     ApplyEncountResult(currentEncounter.GetEncounterNum(), value);

    // }
    // public void LoadEncounter()
    // {
    //     List<Encounter> a = DataController.Inst.LoadEncounterList();
        
    //     int p = 0;
    //     foreach(Encounter value in a)
    //     {
    //         value.SetEncounterNum(p);
    //         ++p;
    //     }
    //     currentEncounter = a[0];
    // }

    private void ApplyEncountResult(int encounterNum, int value)
    {
        switch(encounterNum)
        {
            //모자장수
            case 0:
            switch(value)
            {
                case 0:
                goldMultiple = 2;
                break;

                case 1:
                PlayerManager.Inst.AddAdditionalHealth(5);
                break;

                case 2:
                isIgnorable = false;
                break;
            }
            break;
        }
    }

    private bool TrueOrFalse()
    {
        int value = Random.Range(0,3);
        
        if(value == 0)
        {return true;}
        else
        {return false;}
    }

}

public enum ERoomType
{None, EStair, ESafe, EMonster, EEncount, EItem, EGold}
public enum EDirection
{North,South,West,East}