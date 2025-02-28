using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class DungeonManager : MonoBehaviour
{
    public Camera camera;
    public Sprite stairSprite;
    public Sprite encounterSprite;
    public Sprite monsterSprite;
    
    public Node startNode;
    int width = 14;
    public int waypointFloor = 3;
    public int floor;
    const int floorSize = 140;
    List<Node> map;
    List<GameObject> nodeMap;
    List<int> nodeNumList;

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

    public GameObject popUpMessageWindow;
    public GameObject popUpMessage;

    public List<GameObject> buttonList;

    public TMP_Text floorText;
    public TMP_Text encounterName;
    public TMP_Text encounterDescription;

    public TMP_Text healthText;
    public TMP_Text goldText;

    public TMP_Text textbox;

    private Encounter currentEncounter;

    private int currentPlayerLocation;

    private int goldMultiple;
    private bool isIgnorable;
    public static DungeonManager Inst{get; private set;}
    void Awake() => Inst = this;


    void Start()
    {
        // LoadEncounter();
        // ShowEncounter();
        CreateFloor();
        camera.transform.position = player.transform.position;
        camera.transform.position += new Vector3(0,0,-1);
        
    }


    void Update()
    {
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
        int x = Random.Range(0, nodeNumList.Count);
        currentPlayerLocation = nodeNumList[x];
        
        nodeNumList.Remove(nodeNumList[x]);

        x = Random.Range(0, nodeNumList.Count);
        map[nodeNumList[x]].SetRoomType(ERoomType.EStair);
        nodeNumList.Remove(nodeNumList[x]);

        x = Random.Range(0, nodeNumList.Count);
        map[nodeNumList[x]].SetRoomType(ERoomType.EEncount);
        nodeNumList.Remove(nodeNumList[x]);

        x = Random.Range(0, nodeNumList.Count);
        map[nodeNumList[x]].SetRoomType(ERoomType.EMonster);
        nodeNumList.Remove(nodeNumList[x]);

        for(int i = 0; i < 5; ++i)
        {
            x = Random.Range(0, nodeNumList.Count);

            if(Random.Range(0, 2) == 0)
            {
                map[nodeNumList[x]].SetRoomType(ERoomType.EItem);
                map[nodeNumList[x]].SetItem(new RedPotion(), 2);

            }else
            {
                map[nodeNumList[x]].SetRoomType(ERoomType.EGold);
                map[nodeNumList[x]].SetGold(100);
            }
            nodeNumList.Remove(nodeNumList[x]);
        }
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
                GameObject gameObject = Instantiate(prefab,new UnityEngine.Vector3(), Utils.QI);
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

        // if(roomNum % width == 0)
        // {return false;}

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

        if(floor == waypointFloor)
        {
            wayPointWindow.GetComponent<Window>().OnOff();
            DestroyFloor();
        }
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
        Debug.Log(node.GetGold().ToString() + " " +"골드를 얻었다");
    }

    private void GainItem(Node node)
    {
        Debug.Log(node.GetItem().GetName() + " " +" 를 얻었다");
    }

    public void OpenStairAlert()
    {stairAlert.GetComponent<Window>().OnOff();}

    // public void ShowEncounter()
    // {
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