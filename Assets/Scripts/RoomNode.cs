using System.Collections;
using UnityEngine;
public class RoomNode : MonoBehaviour
{
    Node nodeData;
    public int roomNum;

    public bool filled;

    
    public ERoomType roomType;
    public GameObject roomMark;
    public Renderer renderer;

    public bool upBlocked;
    public bool downBlocked;
    public bool leftBlocked;
    public bool rightBlocked;

    public bool visited;


    void Start()
    {}
    void Awake()
    {
        renderer.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void Decorate(Sprite sprite)
    {
		renderer.GetComponent<SpriteRenderer>().sprite = sprite;
	}

    public Node GetNodeData()
    {return nodeData;}

    public void SetNodeData(Node value)
    {this.nodeData = value;}

    public void SetNodeNum(int value)
    {roomNum = value;}

    public void SetBlocked(bool upBlocked, bool downBlocked, bool leftBlocked, bool rightBlocked)
    {
        this.upBlocked = upBlocked;
        this.downBlocked = downBlocked;
        this.leftBlocked = leftBlocked;
        this.rightBlocked = rightBlocked;

        // UpdateNodeFrame();
    }

    public void SetRoomType(ERoomType roomType)
    {this.roomType = roomType;}

    public ERoomType GetRoomType()
    {return roomType;}

    public void SetWhite()
    {renderer.GetComponent<SpriteRenderer>().color = Color.white;}

    public void SetVisited()
    {
        visited = true;
        renderer.GetComponent<SpriteRenderer>().color = Color.white;

        

        //if (roomType != ERoomType.EStair)
        //{
        //    ClearRoom();
        //}
    }

    public void ClearRoom()
    {
		roomMark.SetActive(false);
		roomType = ERoomType.None;
	}

    public void SetVisited(bool value)
    {
        visited = value;
        if(value)
        {renderer.GetComponent<SpriteRenderer>().color = Color.white;}
    }

    public bool GetVisited()
    {return visited;}

    // public void UpdateNodeFrame()
    // {
    //     if(!upBlocked && rightBlocked && leftBlocked && downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = upDeadEnd;}
    //     else if(upBlocked && !rightBlocked && leftBlocked && downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = rightDeadEnd;}
    //     else if(upBlocked && rightBlocked && !leftBlocked && downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = leftDeadEnd;}
    //     else if(upBlocked && rightBlocked && leftBlocked && !downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = downDeadEnd;}
    //     else if(!upBlocked &&!rightBlocked && leftBlocked && downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = leftDownCorner;}
    //     else if(!upBlocked && rightBlocked && !leftBlocked && downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = rightDownCorner;}
    //     else if(!upBlocked && rightBlocked && leftBlocked && !downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = upDownCorridor;}
    //     else if(upBlocked && !rightBlocked && !leftBlocked && downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = leftRightCorridor;}
    //     else if(upBlocked && !rightBlocked && leftBlocked && !downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = leftUpCorner;}
    //     else if(upBlocked && rightBlocked && !leftBlocked && !downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = rightUpCorner;}
    //     else if(!upBlocked && !rightBlocked && !leftBlocked && downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = downWall;}
    //     else if(!upBlocked && !rightBlocked && leftBlocked && !downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = leftWall;}
    //     else if(!upBlocked && rightBlocked && !leftBlocked && !downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = rightWall;}
    //     else if(upBlocked && !rightBlocked && !leftBlocked && !downBlocked)
    //     {renderer.GetComponent<SpriteRenderer>().sprite = upWall;}
    //     else{Debug.Log(roomNum);}
    // }

    public void UpdateNodeImage(Sprite sprite)
    {roomMark.GetComponent<SpriteRenderer>().sprite = sprite;}
    public void MovePlayer()
    {DungeonManager.Inst.MovePlayer(roomNum);}

    public IEnumerator FadeOut()
    {
        // if (roomType == ERoomType.EWall)
        // {
        //     GetComponent<SpriteMask>().alphaCutoff = 0.6f;
        // }
        float f = 0;
        while (f <= 1)
        {
            f += 0.04f;
            Color ColorAlhpa = renderer.material.color;
            ColorAlhpa.a = f;
            renderer.material.color = ColorAlhpa;
            yield return new WaitForSeconds(0.03f);
        }
        
    }



    void OnMouseEnter()
    {DungeonManager.Inst.SetMouseOnNode(roomNum);}

    // void OnMouseDown()
    // {StartCoroutine(DungeonManager.Inst.FindPath(roomNum));}

    void OnMouseExit()
    {DungeonManager.Inst.ResetMouseOnNode();}
}