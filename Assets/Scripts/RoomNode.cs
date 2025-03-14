using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class RoomNode : MonoBehaviour
{
    Node nodeData;
    public int roomNum;

    
    public ERoomType roomType;
    public GameObject roomMark;
    public Renderer renderer;

    public bool upBlocked;
    public bool downBlocked;
    public bool leftBlocked;
    public bool rightBlocked;

    public bool visited;

    public Sprite leftDownCorner;
    public Sprite leftUpCorner;
    public Sprite rightDownCorner;
    public Sprite rightUpCorner;
    public Sprite leftDeadEnd;
    public Sprite rightDeadEnd;
    public Sprite downDeadEnd;
    public Sprite upDeadEnd;
    public Sprite leftRightCorridor;
    public Sprite upDownCorridor;
    public Sprite leftWall;
    public Sprite rightWall;
    public Sprite downWall;
    public Sprite upWall;

    void Start()
    {renderer.GetComponent<SpriteRenderer>().color = Color.gray;}

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

        UpdateNodeFrame();
    }

    public void CheckUsuable()
    {
        
    }

    public void SetRoomType(ERoomType roomType)
    {this.roomType = roomType;}

    public void SetVisited()
    {
        visited = true;
        renderer.GetComponent<SpriteRenderer>().color = Color.white;

        if(roomType != ERoomType.EStair)
        {
            roomMark.SetActive(false);
            roomType = ERoomType.None;
        }
    }

    public void UpdateNodeFrame()
    {
        if(!upBlocked && rightBlocked && leftBlocked && downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = upDeadEnd;}
        else if(upBlocked && !rightBlocked && leftBlocked && downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = rightDeadEnd;}
        else if(upBlocked && rightBlocked && !leftBlocked && downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = leftDeadEnd;}
        else if(upBlocked && rightBlocked && leftBlocked && !downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = downDeadEnd;}
        else if(!upBlocked &&!rightBlocked && leftBlocked && downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = leftDownCorner;}
        else if(!upBlocked && rightBlocked && !leftBlocked && downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = rightDownCorner;}
        else if(!upBlocked && rightBlocked && leftBlocked && !downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = upDownCorridor;}
        else if(upBlocked && !rightBlocked && !leftBlocked && downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = leftRightCorridor;}
        else if(upBlocked && !rightBlocked && leftBlocked && !downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = leftUpCorner;}
        else if(upBlocked && rightBlocked && !leftBlocked && !downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = rightUpCorner;}
        else if(!upBlocked && !rightBlocked && !leftBlocked && downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = downWall;}
        else if(!upBlocked && !rightBlocked && leftBlocked && !downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = leftWall;}
        else if(!upBlocked && rightBlocked && !leftBlocked && !downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = rightWall;}
        else if(upBlocked && !rightBlocked && !leftBlocked && !downBlocked)
        {renderer.GetComponent<SpriteRenderer>().sprite = upWall;}
        else{Debug.Log(roomNum);}
    }

    public void UpdateNodeImage(Sprite sprite)
    {roomMark.GetComponent<SpriteRenderer>().sprite = sprite;}
    public void MovePlayer()
    {DungeonManager.Inst.MovePlayer(roomNum);}

    public IEnumerator FadeOut()
    {
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
    {
        DungeonManager.Inst.SetMouseOnNode(roomNum);
    }

    void OnMouseExit()
    {DungeonManager.Inst.ResetMouseOnNode();}
}