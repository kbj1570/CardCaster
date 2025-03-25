using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using System;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public Image fadeImage;
    int lineCount;
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
    int enemyLimit;
    int dungeonEndFloor;
    List<int> safeFloorList;
    List<Item> itemDatabase;
    int floor;
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
    public GameObject dungeonEnemyPrefab;
    public GameObject stairAlert;
    public GameObject itemAlert;
    public GameObject wayPointWindow;
    public GameObject dungeonClearWindow;

    public List<GameObject> itemPrefabList;

    public List<Transform> itemLocation;
    public LineRenderer cardDragLine;

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

    int currentPlayerLocation;
    int previousPlayerLocation;

    private int goldMultiple;
    private bool isIgnorable;
    public static DungeonManager Inst{get; private set;}
    int currentPage;
    void Awake()
    {
        moveLocked = true;
        // 이미 존재하는 DungeonManager가 있으면, 새로 생성된 것은 제거
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }

        // 싱글톤 인스턴스로 등록
        Inst = this;
        itemObjectList = new();
        messageList = new();

        enemyObjectList = new();
        dungeonEnemies = new();

        myItemList = new();
        currentItemList = new();
        itemDatabase = DataController.Inst.LoadItemDatabase();
        currentPage = 0;
        lineCount = 30;

        DungeonSetUp();

        if(DungeonData.map == null)
        {
            CreateFloor();
            LoadItemList();

            UpdateItemPage();
        }
        else
        {
            ReCreateFloor();
        }

        StartCoroutine(FadeIn()); //밝아짐짐
        
        //DontDestroyOnLoad(this);
        
    }

    private IEnumerator FadeIn()
    {
        
        float time = 0;
        Color color = fadeImage.color;
        
        while (time < 1f)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, time / 1f); // 알파 값을 1 → 0으로 변경
            fadeImage.color = color;
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
        moveLocked = false;
    }

    private IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        moveLocked = true;
        float time = 0;
        Color color = fadeImage.color;

        while (time < 1f)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, time / 1f); // 알파 값을 0 → 1로 변경
            fadeImage.color = color;
            yield return null;
        }
    }

    List<GameObject> itemObjectList;
    List<GameObject> enemyObjectList;
    public List<GameObject> cardObjectList;


    private float moveDistance = 2f; // 한 번에 이동할 거리
    private float moveDuration = 0.2f; // 이동하는 데 걸리는 시간
    private Queue<Vector2> moveQueue = new Queue<Vector2>(); // 이동할 방향 저장
    private bool isMoving = false;
    private float energyGainLimit = 40;
    private Dictionary<DungeonEnemy, int> dungeonEnemies;
    public IEnumerator FindPath(int target)
    {
        if(moveLocked)
        {yield break;}

        moveLocked = true;
        Queue<int> queue = new Queue<int>();
        Dictionary<int, int> cameFrom = new Dictionary<int, int>();

        queue.Enqueue(currentPlayerLocation);
        cameFrom[currentPlayerLocation] = 0;

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            if (current == target) break;

            foreach (int neighbor in GetNeighborNode(current))
            {
                if (!cameFrom.ContainsKey(neighbor) && nodeNumList.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }
        int temp = target;
        List<string> directions = new List<string>();
        while (temp != 0)
        {

            int prev = cameFrom[temp];

            int dx = temp - prev;

            if (dx == 1) directions.Add("Right");
            else if (dx == -1) directions.Add("Left");
            else if (dx == -width) directions.Add("Up");
            else if (dx == width) directions.Add("Down");

            temp = prev;
        }
        directions.Reverse();

        for(int i = 0; i < directions.Count; ++i)
        {
            previousPlayerLocation = currentPlayerLocation;
            
            switch(directions[i])
            {
                case "Up":
                currentPlayerLocation -= width;
                break;

                case "Down":
                currentPlayerLocation += width;
                break;

                case "Left":
                currentPlayerLocation -= 1;
                break;

                case "Right":
                currentPlayerLocation += 1;
                break;
            }
            MovePlayer(currentPlayerLocation);
            

            switch(directions[i])
            {
                case "Up":
                EnqueueMove(Vector2.up);
                break;

                case "Down":
                EnqueueMove(Vector2.down);
                break;

                case "Left":
                EnqueueMove(Vector2.left);
                break;

                case "Right":
                EnqueueMove(Vector2.right);
                break;
            }
            yield return new WaitForSeconds(0.2f);
        }
        moveLocked = false;
    }

    List<int> GetNeighborNode(int nodeNum)
    {
        List<int> newList = new();

        if(nodeNumList.Contains(nodeNum - 1))
        newList.Add(nodeNum - 1);

        if(nodeNumList.Contains(nodeNum + 1))
        newList.Add(nodeNum + 1);

        if(nodeNumList.Contains(nodeNum - width))
        newList.Add(nodeNum - width);

        if(nodeNumList.Contains(nodeNum + width))
        newList.Add(nodeNum + width);

        return newList;
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

    public void SetEnemyCourse()
    {

        List<DungeonEnemy> keys = dungeonEnemies.Keys.ToList();

        for (int index = 0; index < keys.Count; index++)
        {
            DungeonEnemy enemy = keys[index];

            List<EEnemyDirection> dummy = new();

            if(nodeNumList.Contains(enemy.GetCurrentNodeNum() + 1) && !dungeonEnemies.ContainsValue(enemy.GetCurrentNodeNum() + 1)) 
                dummy.Add(EEnemyDirection.East);
            
            if(nodeNumList.Contains(enemy.GetCurrentNodeNum() - 1) && !dungeonEnemies.ContainsValue(enemy.GetCurrentNodeNum() - 1)) 
                dummy.Add(EEnemyDirection.West);
            
            if(nodeNumList.Contains(enemy.GetCurrentNodeNum() + width) && !dungeonEnemies.ContainsValue(enemy.GetCurrentNodeNum() + width)) 
                dummy.Add(EEnemyDirection.South);
            
            if(nodeNumList.Contains(enemy.GetCurrentNodeNum() - width) && !dungeonEnemies.ContainsValue(enemy.GetCurrentNodeNum() - width)) 
                dummy.Add(EEnemyDirection.North);

            if(dummy.Count == 0)
            {

            }
            else if(enemy.GetMoveLock())
            {
                
            }
            else if(dummy.Count == 1) 
            {
                enemy.SetCurrentNodeNum(ReturnForwardNode(dummy[0], enemy.GetCurrentNodeNum()));
                dungeonEnemies[enemy] = enemy.GetCurrentNodeNum();
                enemy.SetEnemyDirection(dummy[0]);
            }
            else 
            {
                dummy.Remove(ReturnReverseDirection(enemy.GetEnemyDirection()));
                int randomNum = Random.Range(0, dummy.Count);
                enemy.SetCurrentNodeNum(ReturnForwardNode(dummy[randomNum], enemy.GetCurrentNodeNum()));
                dungeonEnemies[enemy] = enemy.GetCurrentNodeNum();
                enemy.SetEnemyDirection(dummy[randomNum]);
            }

            enemy.SetMoveLock(enemy.GetCurrentNodeNum() + 1 == currentPlayerLocation
            || enemy.GetCurrentNodeNum() - 1 == currentPlayerLocation
            || enemy.GetCurrentNodeNum() + width == currentPlayerLocation
            || enemy.GetCurrentNodeNum() - width == currentPlayerLocation);
        }
    }

    public void BackToCampsite()
    {
        SceneManager.LoadScene("Campsite");
        Destroy(gameObject);
    }

    private bool CheckBattleStart()
    {
        if(dungeonEnemies.Values.Contains(currentPlayerLocation)) // 전투 시작
        {return true;}
        else
        {
            List<DungeonEnemy> enemies = dungeonEnemies.Keys.ToList();

            for(int i = 0; i < enemies.Count; ++i)
            {
                if(enemies[i].GetCurrentNodeNum() == previousPlayerLocation
                && EnemyFootStep(enemies[i]) == currentPlayerLocation)
                {return true;}
            }
            return false;
        }
        
        
    }

    public int EnemyFootStep(DungeonEnemy dungeonEnemy)
    {
        switch(dungeonEnemy.GetEnemyDirection())
        {
            case EEnemyDirection.North:
            return dungeonEnemy.GetCurrentNodeNum() + width;

            case EEnemyDirection.West:
            return dungeonEnemy.GetCurrentNodeNum() + 1;

            case EEnemyDirection.East:
            return dungeonEnemy.GetCurrentNodeNum() - 1;

            case EEnemyDirection.South:
            return dungeonEnemy.GetCurrentNodeNum() - width;
        }

        return 0;
    }

    private IEnumerator ReadyBattle()
    {
        moveLocked = true;
        yield return new WaitForSeconds(0.2f);

        int[] offsets = { -1 - width, -width, 1 - width, -1, 0, 1, -1 + width, width, 1 + width };

        List<Enemy> enemies = dungeonEnemies
            .Where(enemy => offsets.Contains(enemy.Value - currentPlayerLocation)) // 반경 1칸 내 적 필터링
            .Where(enemy => CheckConnect(enemy.Key.GetCurrentNodeNum())) // 벽이 없는지 확인
            .Select(enemy => enemy.Key.GetEnemy()) // 최종적으로 적 객체 리스트 변환
            .ToList();

        List<DungeonEnemy> selectedEnemies = dungeonEnemies
            .Where(enemy => offsets.Contains(enemy.Value - currentPlayerLocation)) // 반경 1칸 내 적 필터링
            .Where(enemy => CheckConnect(enemy.Key.GetCurrentNodeNum())) // 벽이 없는지 확인
            .Select(enemy => enemy.Key) // 최종적으로 적 객체 리스트 변환
            .ToList();

       



        foreach(DungeonEnemy selectedEnemy in selectedEnemies)
        {
            enemyObjectList.Remove(selectedEnemy.gameObject);
            dungeonEnemies.Remove(selectedEnemy);
        }

        DungeonData.map = map;
        DungeonData.currentPlayerLocation = currentPlayerLocation;
        DungeonData.previousPlayerLocation = previousPlayerLocation;
        DungeonData.currentFloor = floor;
        DungeonData.nodeNumList = nodeNumList;
        Dictionary<int, bool> activeNodes = new();
        Dictionary<int, bool> visitedNodes = new();

        foreach(int roomNum in nodeNumList)
        {activeNodes.Add(roomNum, nodeMap[roomNum].activeSelf);}

        foreach(int roomNum in nodeNumList)
        {visitedNodes.Add(roomNum, nodeMap[roomNum].GetComponent<RoomNode>().GetVisited());}


        

        DungeonData.activeNodes = activeNodes;
        DungeonData.visitedNodes = visitedNodes;
        StartCoroutine(CameraController.Inst.CameraZoomEffect());
        yield return new WaitForSeconds(0.5f);


        SceneManager.LoadScene("Battle");
        // GameObject[] allObjects = FindObjectsOfType<GameObject>(true); // 비활성화된 오브젝트도 포함하여 검색
        // foreach (GameObject obj in allObjects)
        // {
        //     if(obj != gameObject)
        //     {obj.SetActive(false);}
        // }
        // SceneManager.SetActiveScene(
        //     SceneManager.GetSceneByName("Battle")
        // );
        // StartCoroutine(SetActiveBattleScene());
        
    }

    IEnumerator SetActiveBattleScene()
    {
        yield return new WaitForSeconds(0.2f); // 씬이 로드될 시간을 줌

        SceneManager.SetActiveScene(
            SceneManager.GetSceneByName("Battle")
        );
    }
    
    private bool CheckConnect(int enemyLocation)
    {
        int[] offsets = { 0, 1, width};

        if(offsets.Contains(Math.Abs(enemyLocation - currentPlayerLocation)))
        {return true;}

        int path1 = 0;
        int path2 = 0;

        if(enemyLocation - currentPlayerLocation == 1 + width) // 오른쪽 아래
        {
            path1 = currentPlayerLocation + width;
            path2 = currentPlayerLocation + 1;
        }
        else if(enemyLocation - currentPlayerLocation == 1 - width) // 오른쪽 위
        {
            path1 = currentPlayerLocation - width;
            path2 = currentPlayerLocation + 1;
        }
        else if(enemyLocation - currentPlayerLocation == -1 - width) // 왼쪽 위
        {
            path1 = currentPlayerLocation - width;
            path2 = currentPlayerLocation - 1;
        }
        else if(enemyLocation - currentPlayerLocation == -1 + width) // 왼쪽 아래
        {
            path1 = currentPlayerLocation + width;
            path2 = currentPlayerLocation - 1;
        }


        return nodeNumList.Contains(path1) || nodeNumList.Contains(path2);
    }

    private void MoveEnemy()
    {
        if(dungeonEnemies.Count == 0)
        {return;}
        
        int count = 0;

        foreach(KeyValuePair<DungeonEnemy, int> dungeonEnemy in dungeonEnemies)
        {
            //enemyObjectList[count].transform.position = CalculateNodePosition(dungeonEnemy.Value) + mapObject.transform.position;

            // enemyObjectList[count].transform.DOMove(CalculateNodePosition(dungeonEnemy.Value) + mapObject.transform.position, moveDuration)
            //     .SetEase(Ease.OutQuad)
            //     .OnStart(() => {
            //         if(nodeMap[dungeonEnemy.Key.GetCurrentNodeNum()].GetComponent<RoomNode>().GetVisited() && !dungeonEnemy.Key.gameObject.activeSelf) //방문한 곳이고 자신이 지금 visible false라면?
            //         {StartCoroutine(dungeonEnemy.Key.FadeOut());}

            //     })
            //     .OnComplete(() => {
            //         if(!nodeMap[dungeonEnemy.Key.GetCurrentNodeNum()].GetComponent<RoomNode>().GetVisited() && dungeonEnemy.Key.gameObject.activeSelf) //방문한 곳이 아니고 자신이 지금 visible True라면?
            //         {StartCoroutine(dungeonEnemy.Key.FadeIn());}
            //         });

            enemyObjectList[count].transform.DOMove(CalculateNodePosition(dungeonEnemy.Value) + mapObject.transform.position, moveDuration)
            .SetEase(Ease.OutQuad)
            .OnStart(() => {
                if(nodeMap[dungeonEnemy.Key.GetCurrentNodeNum()].gameObject.activeSelf && !dungeonEnemy.Key.gameObject.activeSelf) //방문한 곳이고 자신이 지금 visible false라면?
                {StartCoroutine(dungeonEnemy.Key.FadeOut());}

            })
            .OnComplete(() => {
                if(!nodeMap[dungeonEnemy.Key.GetCurrentNodeNum()].gameObject.activeSelf && dungeonEnemy.Key.gameObject.activeSelf) //방문한 곳이 아니고 자신이 지금 visible True라면?
                {StartCoroutine(dungeonEnemy.Key.FadeIn());}
                });
            
            count++;
        }
    }



    private int ReturnForwardNode(EEnemyDirection enemyDirection, int nodeNum)
    {
        switch(enemyDirection)
        {
            case EEnemyDirection.North:
            return nodeNum - width;

            case EEnemyDirection.South:
            return nodeNum + width;

            case EEnemyDirection.East:
            return nodeNum + 1;

            case EEnemyDirection.West:
            return nodeNum - 1;
        }
        return -1;
    }

    private EEnemyDirection ReturnReverseDirection(EEnemyDirection enemyDirection)
    {
        switch(enemyDirection)
        {
            case EEnemyDirection.North:
            return EEnemyDirection.South;

            case EEnemyDirection.South:
            return EEnemyDirection.North;

            case EEnemyDirection.East:
            return EEnemyDirection.West;

            case EEnemyDirection.West:
            return EEnemyDirection.East;
        }
        return EEnemyDirection.None;
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
        dungeon = DungeonData.dungeon;
        dungeonName = dungeon.GetDungeonName();
        width = dungeon.GetDungeonWidth();
        height = dungeon.GetDungeonHeight();
        floorSize = dungeon.GetDungeonFloorSize();
        maxGold = dungeon.GetMaxGold();
        dungeonEndFloor = dungeon.GetDungeonEndFloor();
        safeFloorList = dungeon.GetSafeFloorList();
        itemList = dungeon.GetItemList();
        enemyLimit = dungeon.GetEnemyLimit();
    }

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

        if(Input.GetKeyDown(KeyCode.W) && !moveLocked)
        {
            if(CheckOutOfIndex(currentPlayerLocation - width))
            {
                if(nodeMap[currentPlayerLocation - width] != null)
                {
                    previousPlayerLocation = currentPlayerLocation;
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
                    previousPlayerLocation = currentPlayerLocation;
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
                    previousPlayerLocation = currentPlayerLocation;
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
                    previousPlayerLocation = currentPlayerLocation;
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
        {StartNextMove();}
    }

    void StartNextMove()
    {
        if (moveQueue.Count > 0)
        {
            isMoving = true;
            Vector2 direction = moveQueue.Dequeue();

        // 

            Vector3 targetPosition = player.transform.position + (Vector3)direction * moveDistance;

            
            SetEnemyCourse();

            player.transform.DOMove(targetPosition, moveDuration)
                .SetEase(Ease.OutQuad) // 부드러운 감속 효과
                .OnComplete(() => {
                    isMoving = false;
                    StartNextMove(); // 다음 이동 실행
                });

            // if(CheckBattleStart())
            // {moveDistance = moveDistance / 2;}

            MoveEnemy();
            if(CheckBattleStart())
            {StartCoroutine(ReadyBattle());}
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

        // x = Random.Range(0, nodeNumList.Count);
        // map[nodeNumList[x]].SetRoomType(ERoomType.EMonster);
        // SetEnemyInNode(map[nodeNumList[x]]);

        // x = Random.Range(0, nodeNumList.Count);
        // map[nodeNumList[x]].SetRoomType(ERoomType.EMonster);
        // SetEnemyInNode(map[nodeNumList[x]]);

        

        // 중복되지 않는 랜덤숫자 생성

        List<int> usedNumbers = new List<int>();

        for(int i = 0; i < 5; ++i)
        {
            int num;
            do {num = Random.Range(0, nodeNumList.Count);}
            while (usedNumbers.Contains(num));
            
            usedNumbers.Add(nodeNumList[num]);
        }

        for(int i = 0; i < usedNumbers.Count; ++i)
        {

            if(Random.Range(0, 2) == 0)
            {
                map[usedNumbers[i]].SetRoomType(ERoomType.EItem);
                map[usedNumbers[i]].SetItem(ReturnDungeonItem(), Random.Range(0, 2));

            }else
            {
                map[usedNumbers[i]].SetRoomType(ERoomType.EGold);
                map[usedNumbers[i]].SetGold(Random.Range(1, maxGold + 1));
            }
        }



        x = Random.Range(0, nodeNumList.Count);
        map[nodeNumList[x]].SetRoomType(ERoomType.EStair);

        x = Random.Range(0, nodeNumList.Count);

        while(map[nodeNumList[x]].GetRoomType() != ERoomType.None)
        {x = Random.Range(0, nodeNumList.Count);}


        currentPlayerLocation = nodeNumList[x];
        
        previousPlayerLocation = currentPlayerLocation;
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

    public void ReCreateFloor()
    {
        
        map = DungeonData.map;
        currentPlayerLocation = DungeonData.currentPlayerLocation;
        previousPlayerLocation = DungeonData.previousPlayerLocation;
        floor = DungeonData.currentFloor;
        floorText.text = floor.ToString() + "F";
        
        nodeNumList = DungeonData.nodeNumList;

        InstantiateNode();

        foreach(KeyValuePair<int, bool> keyValuePair in DungeonData.activeNodes)
        {nodeMap[keyValuePair.Key].SetActive(keyValuePair.Value);}

        foreach(KeyValuePair<int, bool> keyValuePair in DungeonData.visitedNodes)
        {nodeMap[keyValuePair.Key].GetComponent<RoomNode>().SetVisited(keyValuePair.Value);}

        MovePlayer(currentPlayerLocation);
        nodeMap[currentPlayerLocation].GetComponent<RoomNode>().SetVisited();
        player.transform.position = CalculateNodePosition(currentPlayerLocation) + mapObject.transform.position;
        camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);
       
        LoadItemList();
        UpdateItemPage();
    }

    public void LoadData()
    {

    }

    public void CreateFloor()
    {
        nodeNumList = new List<int>();
        while(nodeNumList.Count < 40) // 생성된 층의 크기가 10보다 작으면 다시 만듬
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
        nodeMap[currentPlayerLocation].GetComponent<RoomNode>().SetWhite();
        nodeMap[currentPlayerLocation].GetComponent<RoomNode>().SetVisited();
        player.transform.position = CalculateNodePosition(currentPlayerLocation) + mapObject.transform.position;
        camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);
        CreateEnemy();
    }

    public void CreateEnemy()
    {
        // 우선 랜덤으로 배치한다.
        List<int> usedNumbers = new List<int>();
        dungeonEnemies.Clear();

        for(int i = 0; i < enemyLimit; ++i)
        {
            int num;
            do {num = Random.Range(0, nodeNumList.Count);}
            while (usedNumbers.Contains(num) || nodeNumList[num] == currentPlayerLocation);
            
            usedNumbers.Add(nodeNumList[num]);
        }

        for(int i = 0; i < enemyLimit; ++i)
        {
            GameObject enemyObject = Instantiate(dungeonEnemyPrefab);

            enemyObject.GetComponent<DungeonEnemy>().SetEnemy(new UnknownMonster());
            enemyObject.GetComponent<DungeonEnemy>().SetCurrentNodeNum(usedNumbers[i]);

            List<EEnemyDirection> dummy = new();

            if(nodeNumList.Contains(usedNumbers[i] + 1)) 
                dummy.Add(EEnemyDirection.East);
            
            if(nodeNumList.Contains(usedNumbers[i] - 1)) 
                dummy.Add(EEnemyDirection.West);
            
            if(nodeNumList.Contains(usedNumbers[i] + width)) 
                dummy.Add(EEnemyDirection.South);
            
            if(nodeNumList.Contains(usedNumbers[i] - width)) 
                dummy.Add(EEnemyDirection.North);

            dungeonEnemies.Add(enemyObject.GetComponent<DungeonEnemy>(), usedNumbers[i]);

            int randomNum = Random.Range(0, dummy.Count);
            enemyObject.GetComponent<DungeonEnemy>().SetEnemyDirection(dummy[randomNum]);

            enemyObjectList.Add(enemyObject);

            enemyObject.GetComponent<DungeonEnemy>().SetVisible(nodeMap[enemyObject.GetComponent<DungeonEnemy>().GetCurrentNodeNum()]
            .GetComponent<RoomNode>().GetVisited());
            enemyObject.GetComponent<DungeonEnemy>().SetMoveLock(
                dungeonEnemies[enemyObject.GetComponent<DungeonEnemy>()] + 1 == currentPlayerLocation
            || dungeonEnemies[enemyObject.GetComponent<DungeonEnemy>()] - 1 == currentPlayerLocation
            || dungeonEnemies[enemyObject.GetComponent<DungeonEnemy>()] + width == currentPlayerLocation
            || dungeonEnemies[enemyObject.GetComponent<DungeonEnemy>()] - width == currentPlayerLocation
            );

        }
        MoveEnemy();        
    }

    public void DestroyFloor()
    {
        foreach(GameObject node in nodeMap)
        {Destroy(node);}

        foreach(GameObject enemyObject in enemyObjectList)
        {
            Destroy(enemyObject);
        }
        enemyObjectList.Clear();

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
            }
        }
    }

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


    private void CreateRoomNode(int roomNum)
    {
        if(!CheckOutOfIndex(roomNum))
        {return;}

        if(map[roomNum] != null)
        {return;}
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
        moveLocked = false;
        StartCoroutine(ReadyNextFloor());

    }

    private IEnumerator ReadyNextFloor()
    {
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(1f);
        if(safeFloorList.Contains(floor))
        {
            wayPointWindow.GetComponent<Window>().OnOff();
            DestroyFloor();
        }
        else if(dungeonEndFloor == floor)
        {
            Debug.Log("던전을 클리어 했습니다");
            dungeonClearWindow.GetComponent<Window>().OnOff();
            DungeonData.Reset();
        }
        else
        {
            DestroyFloor();
            CreateFloor();
            CameraController.Inst.SetFollowing();
        }


        StartCoroutine(FadeIn());
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

    public void SelectUsingItem(int itemNum, int itemOrder)
    {
        clickedItemOrder = itemOrder + (currentPage * 8);
        clickedItem = myItemList[clickedItemOrder];
        itemAlert.GetComponent<Window>().OnOff();
        itemAlert.GetComponent<ItemAlert>().SetText(clickedItem.GetName());
    }
    

    public void MovePlayer(int roomNum)
    {
        foreach(GameObject card in cardObjectList)
        {card.GetComponent<DungeonCard>().AddEnergy(1f);}

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
        {map[roomNum].SetRoomType(ERoomType.None);}
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

    public void CardBeginDrag(GameObject cardObject)
    {
        foreach(GameObject card in cardObjectList)
        {card.GetComponent<DungeonCard>().SetLock(true);}
        cardObject.GetComponent<DungeonCard>().SetLock(false);

        CameraController.Inst.SetFollowing();
        CameraController.Inst.SetDragLock(true);

    }

    public void CardOnDrag(GameObject cardObject)
    {DrawDragLine(cardObject.transform.position,CheckCardUsable(cardObject.GetComponent<DungeonCard>().GetCardData(),ReturnMouseOnNode()));}

    public void DrawDragLine(Vector2 startPoint, bool isUsuable)
    {
         Vector3[] point = new Vector3[lineCount];
        float posA = 3f;
        float posB = 3f;
        cardDragLine.positionCount = lineCount;

        if(isUsuable)
        {cardDragLine.endColor = Color.blue;}
        else
        {cardDragLine.endColor = Color.red;}
        
        Vector3 targetPoint = camera.ScreenToWorldPoint(Input.mousePosition);

        startPoint = camera.ScreenToWorldPoint(startPoint);

        for(int i = 0; i < lineCount; ++i)
        {
            float t;
            if (i == 0)
            {t = 0;}
            else
            {t = (float)i / (lineCount - 1);}
            
            point[i] = Bezier(startPoint, PointSetting(startPoint),
            PointSetting(targetPoint),targetPoint, t);
            point[i].z = 0;
        }
        cardDragLine.SetPositions(point);
        

        Vector3 PointSetting(Vector3 origin){
            float x, y;
            x = posA * Mathf.Cos(120 * Mathf.Deg2Rad) + origin.x;
            y = posB * Mathf.Sin(120 * Mathf.Deg2Rad) + origin.y;
    
            return new Vector3(x, y);
        }
        Vector3 Bezier(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
        {
            Vector3 M0 = Vector3.Lerp(P0, P1, t);
            Vector3 M1 = Vector3.Lerp(P1, P2, t);
            Vector3 M2 = Vector3.Lerp(P2, P3, t);

            Vector3 B0 = Vector3.Lerp(M0, M1, t);
            Vector3 B1 = Vector3.Lerp(M1, M2, t);

            return Vector3.Lerp(B0, B1, t);
        }
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

        
        CameraController.Inst.SetDragLock(false);
        
        yield return new WaitForSeconds(0.5f);

    }

    public bool CheckCardUsable(CardData cardData, int nodeNum)
    {
        if(mouseOnRoomNum == 0)
        {return false;}

        return true;
    }

    public void GainEnergyToMax()
    {
        foreach(GameObject card in cardObjectList)
        {
            card.GetComponent<DungeonCard>().AddEnergy(1000);
        }
    }

    //나와 적 사이가 항상 1칸이 되게 유지



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