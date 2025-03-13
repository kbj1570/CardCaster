using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class DungeonManager : MonoBehaviour
{
    int mouseOnRoomNum;
    bool moveLocked;
    Item clickedItem;
    int clickedItemOrder;
    GameObject clickedItemInfo;
    public GameObject itemDescriptionWindow;
    private int pageLimit;
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
    public GameObject itemAlert;
    public GameObject wayPointWindow;

    public List<GameObject> itemPrefabList;

    public List<Transform> itemLocation;
    public LineRenderer cardDragLine;

    // private Dictionary<Item, int> currentItemList;
    // private Dictionary<Item, int> myItemList;

    private List<Item> currentItemList;
    private List<Item> myItemList;

    public GameObject popUpMessageWindow;
    public GameObject popUpMessage;

    public GameObject nextButton;
    public GameObject backButton;

    public List<GameObject> itemInfoPrefab;

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
    List<GameObject> cardObjectList;


    private float moveDistance = 2f; // 한 번에 이동할 거리
    private float moveDuration = 0.2f; // 이동하는 데 걸리는 시간
    private Queue<Vector2> moveQueue = new Queue<Vector2>(); // 이동할 방향 저장
    private bool isMoving = false;


    private void Start()
    {
        
        DungeonSetUp();
        CreateFloor();

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

    public void RevealMap()
    {
        foreach(GameObject node in nodeMap)
        {
            if(node != null)
            {
                node.SetActive(true);
                // node.GetComponent<RoomNode>().SetVisited();
            }
        }
    }

    public void SetDungeon(Dungeon dungeon)
    {this.dungeon = dungeon;}

    public void SetSelectedItem(int itemNum)
    {selectedItem = itemDatabase[itemNum];}

    public void LoadItemList()
    {
        foreach(string value in DataController.Inst.LoadItemList())
        {myItemList.Add(itemDatabase[Convert.ToInt32(value)]);}
    }

    public void ChangePage(bool value)
    {
        if(value)
        {currentPage++;}
        else{currentPage--;}

        if(currentPage < 0)
        {currentPage = 0;}

        if(currentPage >= pageLimit)
        {currentPage = pageLimit;}

        UpdateItemPage();
    }

    public void UpdateItemPage()
    {
        int count = 0;
        int sum = myItemList.Count;
        pageLimit = (sum / 8);
        int remainder = sum % 8;

        currentItemList.Clear();
        foreach(GameObject gameObject in itemObjectList)
        {Destroy(gameObject);}

        if(sum == 0)
        {return;}

        if(pageLimit < currentPage)
        {currentPage--;}

        if(remainder == 0)
        {
            pageLimit--;
            remainder = 8;
        }

        if(currentPage != pageLimit)
        {
            for(int i = 0; i < 8; ++i)
            {currentItemList.Add(myItemList[(currentPage * 8) + i]);}
        }else
        {
            for(int i = 0; i < remainder; ++i)
            {currentItemList.Add(myItemList[(currentPage * 8) + i]);}
        }

        foreach(Item item in currentItemList)
        {
            int x = Convert.ToInt32(item.GetNum());
            GameObject cardObject = Instantiate(itemPrefabList[x],
            new Vector3(0,0,0) , Utils.QI);
            cardObject.transform.SetParent(itemLocation[count].transform);
            cardObject.transform.localScale = new Vector3(1f,1f,1f);
            cardObject.transform.localPosition = new Vector3(0,0,0);
            
            itemObjectList.Add(cardObject);

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
    }

    public void ShowItemDescription(int itemNum)
    {
        itemDescriptionWindow.SetActive(true);
        itemDescriptionWindow.GetComponent<DescriptionWindow>().SetUp(itemDatabase[itemNum].GetName(),
        itemDatabase[itemNum].GetItemDescription());
    }

    public void HideItemDescription()
    {
        itemDescriptionWindow.SetActive(false);
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
        // {MoveToNextTile();}

        if(Input.GetKeyDown(KeyCode.W) && !moveLocked)
        {
            if(CheckOutOfIndex(currentPlayerLocation - width))
            {
                if(nodeMap[currentPlayerLocation - width] != null)
                {
                    currentPlayerLocation -= width;
                    MovePlayer(currentPlayerLocation);
                    EnqueueMove(Vector2.up);
                }
            }
            CameraController.Inst.SetFollowing();
        }


        if(Input.GetKeyDown(KeyCode.A) && !moveLocked)
        {
            if(currentPlayerLocation % width == 0)
            return;

            if(CheckOutOfIndex(currentPlayerLocation - 1))
            {
                if(nodeMap[currentPlayerLocation - 1] != null)
                {
                    currentPlayerLocation -= 1;
                    MovePlayer(currentPlayerLocation);
                    EnqueueMove(Vector2.left);
                }
            }
            CameraController.Inst.SetFollowing();
        }

        if(Input.GetKeyDown(KeyCode.S) && !moveLocked)
        {
            if(CheckOutOfIndex(currentPlayerLocation + width))
            {
                if(nodeMap[currentPlayerLocation + width] != null)
                {
                    currentPlayerLocation += width;
                    MovePlayer(currentPlayerLocation);
                    EnqueueMove(Vector2.down);
                }
            }
            CameraController.Inst.SetFollowing();
        }

        if(Input.GetKeyDown(KeyCode.D) && !moveLocked)
        {
            if(currentPlayerLocation % width == width - 1)
            return;

            if(CheckOutOfIndex(currentPlayerLocation + 1))
            {
                if(nodeMap[currentPlayerLocation + 1] != null)
                {
                    currentPlayerLocation += 1;
                    MovePlayer(currentPlayerLocation);
                    
                    EnqueueMove(Vector2.right);
                }
            }
            CameraController.Inst.SetFollowing();
        }
    }

     void EnqueueMove(Vector2 direction)
    {
        moveQueue.Enqueue(direction);
        if (!isMoving)
        {
            StartNextMove();
        }
    }

    void StartNextMove()
    {
        if (moveQueue.Count > 0)
        {
            isMoving = true;
            Vector2 direction = moveQueue.Dequeue();
            Vector3 targetPosition = player.transform.position + (Vector3)direction * moveDistance;

            player.transform.DOMove(targetPosition, moveDuration)
                .SetEase(Ease.OutQuad) // 부드러운 감속 효과
                .OnComplete(() => {
                    isMoving = false;
                    StartNextMove(); // 다음 이동 실행
                });
        }
    }

    public void CloseItemInfo()
    {
        if(clickedItem == null)
        {return;}

        if(clickedItem != null)
        {
            clickedItem = null;
            Destroy(clickedItemInfo.gameObject);
        }
    }

    
    private void SetNodeRoom()
    {
         

        int x = Random.Range(0, nodeNumList.Count);
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

        x = Random.Range(0, nodeNumList.Count);

        while(map[nodeNumList[x]].GetRoomType() != ERoomType.None)
        {x = Random.Range(0, nodeNumList.Count);}

        currentPlayerLocation = nodeNumList[x];
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
            map = new(); // Node들의 정보를 담은 map
            for(int i = 0; i < floorSize; ++i)
            {map.Add(null);} // 플로어의 크기만큼 null로 채워넣음
            CreateFirstRoom();
            CreateNodeNumList();
        }


        floorText.text = floor.ToString() + "F";
        AddLoopCorridor();
        CreateNodeNumList();
        SetNodeRoom();
        InstantiateNode();
        
        MovePlayer(currentPlayerLocation);
        player.transform.position = CalculateNodePosition(currentPlayerLocation) + mapObject.transform.position;
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

                 nodeMap[i] =gameObject;
                // nodeMap.Insert(i, gameObject);
            }
        }
    }

    private void CreateFirstRoom()
    {
        int roomNum = floorSize / 2; //중간에서 첫번쨰 방 생성
        map[roomNum] = new Node();
        map[roomNum].SetRoomType(ERoomType.None);

        CreateRoomNode(roomNum + 1);
        CreateRoomNode(roomNum - 1);
        CreateRoomNode(roomNum + width);
        CreateRoomNode(roomNum - width);
    }


    private void CreateRoomNode(int roomNum)
    {
        if(!CheckOutOfIndex(roomNum))
        {return;}

        if(map[roomNum] != null)
        {return;}

        // if(roomNum / width == 0)
        // {return;}

        // if((roomNum + 1) / width != roomNum + 1 / width)
        // return;

        // if((roomNum - 1) / width != roomNum - 1 / width)
        // return;

        if(roomNum % width == width - 1)
        return;

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
            // if(!CheckCorridor(i))
            // {nodeNumList.Add(i);}
            if(map[i] != null)
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
            // camera.transform.position = player.transform.position;
            // camera.transform.position += new Vector3(0,0,-1);
        }
    }

    public void RemoveClickedItem()
    {
        myItemList.RemoveAt(clickedItemOrder);
        UpdateItemPage();
    }

    public void UseItem()
    {

        switch(clickedItem.GetNum())
        {
            case "1": // 거대한포션
            PlayerManager.Inst.GainHealth(5);
            break;

            case "2": // 황금주사위
            
            break;

            case "3": // 부숴진나침반
            
            break;

            case "4": // 불길한향로
            
            break;

            case "5": // 불길한향로
            
            break;

            case "6": // 빨간 포션
            PlayerManager.Inst.GainHealth(2);
            break;
        }

        AlertPopUpMessage(clickedItem.GetName() + "을(를) 사용하였습니다.");
    }

    public void IncreaseSelectionValue()
    {selectionValue++;}
    public void DecreaseSelectionValue()
    {selectionValue--;}
    public int GetSelectionValue()
    {return selectionValue;}

    public void SelectUsingItem(int itemNum, int itemOrder)
    {
        clickedItemOrder = itemOrder + (currentPage * 8);
        clickedItem = myItemList[clickedItemOrder];
        itemAlert.GetComponent<Window>().OnOff();
        itemAlert.GetComponent<ItemAlert>().SetText(clickedItem.GetName());
    }
    

    public void MovePlayer(int roomNum)
    {
        // player.transform.position = CalculateNodePosition(roomNum) + mapObject.transform.position;
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
        {ShowStairAlert();}
        else if(map[roomNum].GetRoomType() == ERoomType.EMonster)
        {map[roomNum].SetRoomType(ERoomType.None);}
        else if(map[roomNum].GetRoomType() == ERoomType.EEncount)
        {
            map[roomNum].SetRoomType(ERoomType.None);}
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
    {AlertPopUpMessage(node.GetGold().ToString() + " " +"골드 획득");}

    private void GainItem(Node node)
    {
        AlertPopUpMessage(node.GetItem().GetName() + " " +" 획득");
        myItemList.Add(node.GetItem());
        UpdateItemPage();
    }

    public void ShowStairAlert()
    {
        moveLocked = true;
        stairAlert.GetComponent<Window>().OnOff();
    }

    public void HideStairAlert()
    {
        moveLocked = false;
        stairAlert.GetComponent<Window>().OnOff();
    }

    public void CardBeginDrag(GameObject gameObject)
    {

    }

    public void CardOnDrag(GameObject gameObject)
    {

    }

    IEnumerator ActivateSpell(CardData cardData, int nodeNum)
    {

        yield return new WaitForSeconds(1.5f);
    }

    public void DeleteDragLine()
    {
        cardDragLine.positionCount = 0;
        cardDragLine.endColor = Color.blue;
    }

    public IEnumerator CardEndDrag(DungeonCard dungeonCard, int nodeNum)
    {
        yield return new WaitForSeconds(0.5f);

        foreach(GameObject cardObject in cardObjectList)
        {cardObject.GetComponent<DungeonCard>().SetLock(false);}

        DeleteDragLine();

        if(CheckCardUsable(dungeonCard.GetCardData(), nodeNum))
        {
            StartCoroutine(ActivateSpell(dungeonCard.GetCardData(), nodeNum));
            // card.SendMissile(alertPoint, hole.transform);

            for(int i = 0; i < cardObjectList.Count; ++i)
            {cardObjectList[i].GetComponent<DungeonCard>().SetCardOrder(i);}
            // CardAlignmentAlt();
        }
        else
        {
            foreach(GameObject cardObject in cardObjectList)
            {cardObject.GetComponent<DungeonCard>().SetLock(false);}
        }
    }

    public bool CheckCardUsable(CardData cardData, int nodeNum)
    {
        if(mouseOnRoomNum == 0)
        {return false;}

        return true;
    }

    public int ReturnMouseOnNode()
    {return mouseOnRoomNum;}

    public void SetMouseOnNode(int roomNum)
    {mouseOnRoomNum = roomNum;}

    public void ResetMouseOnNode()
    {mouseOnRoomNum = 0;}

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