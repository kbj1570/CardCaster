using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using DG.Tweening;
using Unity.Mathematics;

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
    bool flag;
    List<Node> horizontalNodeLine;
    List<Node> map;
    List<GameObject> nodeMap;

    List<int> openList;
    List<int> closeList;
    List<int> nodeNumList;

    public GameObject nodePrefab;
    public GameObject mapObject;
    public GameObject buttonPrefab;
    public GameObject player;
    public GameObject stairAlert;
    public GameObject wayPointWindow;

    public List<GameObject> buttonList;

    public TMP_Text floorText;
    public TMP_Text encounterName;
    public TMP_Text encounterDescription;

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

        MovePlayer(currentPlayerLocation);
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
        InstantiateNode();
        
        CreateNodeNumList();
        // UpdateNodeBlocked();
        SetNodeRoom();
        UpdateNode();
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

    private void UpdateNode()
    {
        for(int i = 0; i < nodeNumList.Count; ++i)
        {
            switch(map[nodeNumList[i]].GetRoomType())
            {
                case ERoomType.EStair:
                nodeMap[nodeNumList[i]].GetComponent<RoomNode>().UpdateNodeImage(stairSprite);
                break;

                case ERoomType.EMonster:
                nodeMap[nodeNumList[i]].GetComponent<RoomNode>().UpdateNodeImage(monsterSprite);
                break;

                case ERoomType.EEncount:
                nodeMap[nodeNumList[i]].GetComponent<RoomNode>().UpdateNodeImage(encounterSprite);
                break;

                default:
                break;
            }
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
                GameObject gameObject = Instantiate(nodePrefab,new UnityEngine.Vector3(), Utils.QI);
                gameObject.transform.SetParent(mapObject.transform);
                gameObject.transform.position = mapObject.transform.position;
                gameObject.GetComponent<RoomNode>().SetNodeNum(i);
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

        if(roomNum / width == 0)
        {return;}

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

    private UnityEngine.Vector3 CalculateNodePosition(int roomNum)
    {
        int x = ((roomNum  % width) - 9) * 2;
        int y = -(((roomNum / width) - 8) * 2);

        return new UnityEngine.Vector3(x, y, 0);
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

        if(roomNum % width == 0)
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

    // public void RotateFloor()
    // {
    //     List<List<Node>> newNodeMap = new();
    //     List<Node> nullNodes;


    //     for(int p = 0; p < height; ++p)
    //     {
    //         nullNodes = new();
    //         for(int q = 0; q < width; ++q)
    //         {
    //             nullNodes.Add(null);
    //         }
    //         newNodeMap.Add(nullNodes);
    //     }

    //     int center = (width - 1) / 2;

    //     for(int p = 0; p < height; ++p)
    //     {
    //         for(int q = 0; q < width; ++q)
    //         {newNodeMap[q][(2 * center) - p] = allNodeMap[p][q];}
    //     }

    //     allNodeMap = newNodeMap;

    //     for(int p = 0; p < height; ++p)
    //     {
    //         for(int q = 0; q < width; ++q)
    //         {
    //             Node node = allNodeMap[p][q];
    //             if(node != null)
    //             {
    //                 Node eastNode = node.GetEastNode();
    //                 node.SetEastNode(node.GetNorthNode());
    //                 node.SetNorthNode(node.GetWestNode());
    //                 node.SetWestNode(node.GetSouthNode());
    //                 node.SetSouthNode(eastNode);

    //             }
    //         }
    //     }
    // }

    // public void CreateFloor()
    // {
    //     allNodeMap = new();
    //     CreateNodeLine();

    //     for(int i = 1; i < height; ++i)
    //     {
    //         CreateNodeLine();
    //         ConnectNodeLine(allNodeMap[i - 1], allNodeMap[i]);
    //     }

    //     WeedOutNode(10);

    //     floorSize = 0;
    //     tree_Inorder(allNodeMap[0][0]);

    //     if(allNodeMap[height - 1][width - 1].GetCreated())
    //     {ConnectFloor();}
    //     else
    //     {CreateFloor();}
    // }

    // public void WeedOutNode(int count)
    // {
    //     int x;
    //     int y;

    //     for(int p = 0; p < count; ++p)
    //     {
    //         x = Random.Range(0, width);
    //         y = Random.Range(0, height);

    //         if(x + y == 0 || x + y == width + height)
    //         {++count;}
    //         else
    //         {DestroyNode(allNodeMap[x][y]);}
    //     }

        
    // }

    // public void ConnectFloor()
    // {

    //     RotateFloor();
    //     RotateFloor();

    //     tree_Inorder(allNodeMap[0][0]);

    //     RotateFloor();
    //     RotateFloor();

    //     Debug.Log(floorSize);
    // }

    // public void CreateNodeLine()
    // {

    //     horizontalNodeLine = new();
    //     Node newNode = new();
    //     Node firstNode = new();
    //     horizontalNodeLine.Add(firstNode);

    //     ConnectNode(firstNode, newNode, EDirection.West);

    //     Node previousNode = newNode;
    //     horizontalNodeLine.Add(newNode);
    //     for(int i = 0; i < width - 2; ++i)
    //     {
    //         newNode = new();
    //         horizontalNodeLine.Add(newNode);
    //         ConnectNode(previousNode, newNode, EDirection.West);
    //         previousNode = previousNode.GetEastNode();
    //     }

    //     allNodeMap.Add(horizontalNodeLine);
    // }

    // public void initial_Tree_Inorder(Node node)
    // {
    //     if (node != null) {
    //         CreateNode(node);
    //         tree_Inorder(node.GetSouthNode());// 왼쪽서브트리 순회
    //         tree_Inorder(node.GetEastNode());// 오른쪽서브트리 순회
    //         tree_Inorder(node.GetWestNode());
    //         tree_Inorder(node.GetNorthNode());
    //     }
    // }

    // public void tree_Inorder(Node node) {
    //     if(node == null)
    //     {return;}

    //     // if(node.GetConnectCount() == 1)
    //     // {
    //     //     CreateNode(node);
    //     //     return;
    //     // }

    //     if(!node.GetCreated())
    //     {
    //         node.SetCreated(true);
    //         GameObject gameObject = Instantiate(nodePrefab, new Vector3() , Utils.QI);
    //         gameObject.transform.SetParent(mapObject.transform);

    //         floorSize++;
    //     }

    //     tree_Inorder(node.GetSouthNode());// 왼쪽서브트리 순회
    //     tree_Inorder(node.GetEastNode());// 오른쪽서브트리 순회
    // }

    public void MovePlayer(int roomNum)
    {
        player.transform.position = CalculateNodePosition(roomNum) + mapObject.transform.position;
        nodeMap[roomNum].SetActive(true);
        nodeMap[roomNum].GetComponent<RoomNode>().SetVisited();

        if(CheckOutOfIndex(roomNum + 1))
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

        if(CheckOutOfIndex(roomNum - 1))
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
        {}
        else if(map[roomNum].GetRoomType() == ERoomType.EEncount)
        {}
    }

    public void OpenStairAlert()
    {stairAlert.GetComponent<Window>().OnOff();}

    // public Node SearchNode(Node firstNode, int x, int y)
    // {return allNodeMap[x][y];}

    // public void DestroyNode(RoomNode roomNode)
    // {
    //     Node node = roomNode.GetNodeData();
    //     DisConnectNode(node, node.GetNorthNode());
    //     DisConnectNode(node, node.GetSouthNode());
    //     DisConnectNode(node, node.GetWestNode());
    //     DisConnectNode(node, node.GetEastNode());

    //     Destroy(roomNode);
    // }

    // public void DestroyNode(Node node)
    // {
    //     if(node == null)
    //     {return;}

    //     allNodeMap[1][0] = null;

    //     DisConnectNode(node, node.GetNorthNode());
    //     DisConnectNode(node, node.GetSouthNode());
    //     DisConnectNode(node, node.GetWestNode());
    //     DisConnectNode(node, node.GetEastNode());
    // }

    // public void ConnectNodeLine(List<Node> first, List<Node> second)
    // {
    //     for(int i = 0; i < width; i++)
    //     {ConnectNode(first[i], second[i], EDirection.North);}
    // }

    // public void ConnectNode(Node start, Node end, EDirection direction)
    // {
    //     switch(direction)
    //     {
    //         case EDirection.North:
    //         start.SetSouthNode(end);
    //         end.SetNorthNode(start);
    //         break;

    //         case EDirection.South:
    //         start.SetNorthNode(end);
    //         end.SetSouthNode(start);
    //         break;

    //         case EDirection.West:
    //         start.SetEastNode(end);
    //         end.SetWestNode(start);
    //         break;

    //         case EDirection.East:
    //         start.SetWestNode(end);
    //         end.SetEastNode(start);
    //         break;
    //     }

    // }

    // public void DisConnectNode(Node start, Node end)
    // {

    //     if(end == null)
    //     {return;}

    //     if(start.GetEastNode() == end)
    //     {
    //         start.SetEastNode(null);
    //         end.SetWestNode(null);
    //     }
    //     else if(start.GetWestNode() == end)
    //     {
    //         start.SetWestNode(null);
    //         end.SetEastNode(null);
    //     }
    //     else if(start.GetNorthNode() == end)
    //     {
    //         start.SetNorthNode(null);
    //         end.SetSouthNode(null);
    //     }
    //     else if(start.GetSouthNode() == end)
    //     {
    //         start.SetSouthNode(null);
    //         end.SetNorthNode(null);
    //     }
    // }
    // public Boolean CheckConnected(Node node)
    // {

    //     if(node == null)
    //     {return true;}

    //     if(node.GetEastNode() == null && node.GetNorthNode() == null &&
    //     node.GetWestNode() == null && node.GetSouthNode() == null)
    //     {return false;}

    //     return false;
    // }
    public void ShowEncounter()
    {
        buttonList[0].SetActive(true);
        buttonList[1].SetActive(false);
        buttonList[2].SetActive(false);
        buttonList[3].SetActive(true);

        encounterName.text = currentEncounter.GetEncounterName();
        encounterDescription.text = currentEncounter.GetEncounterDescription();

        List<string> select = currentEncounter.GetSelect();

        buttonList[0].GetComponent<MyButton>().SetText(select[0]);

        switch(select.Count)
        {
            case 2:
            buttonList[1].SetActive(true);
            buttonList[1].GetComponent<MyButton>().SetText(select[1]);
            break;

            case 3:
            buttonList[1].SetActive(true);
            buttonList[1].GetComponent<MyButton>().SetText(select[1]);

            buttonList[2].SetActive(true);
            buttonList[2].GetComponent<MyButton>().SetText(select[2]);
            break;
        }
    }

    public void ShowResult(int value)
    {
        buttonList[0].SetActive(false);
        buttonList[1].SetActive(false);
        buttonList[2].SetActive(false);
        buttonList[3].SetActive(false);

        encounterDescription.text = currentEncounter.GetResult()[value];

        ApplyEncountResult(currentEncounter.GetEncounterNum(), value);

    }
    public void LoadEncounter()
    {
        List<Encounter> a = DataController.Inst.LoadEncounterList();
        
        int p = 0;
        foreach(Encounter value in a)
        {
            value.SetEncounterNum(p);
            ++p;
        }
        currentEncounter = a[0];
    }

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
{None, EStair, ESafe, EMonster, EEncount}
public enum EDirection
{North,South,West,East}