using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.UI;
public class RoomNode : MonoBehaviour
{
    Node nodeData;
    public TMP_Text nodeNum;
    public int roomNum;
    public Image roomTypeImage;

    public Node GetNodeData()
    {return nodeData;}

    public void SetNodeData(Node value)
    {this.nodeData = value;}

    public void SetNodeNum(int value)
    {
        roomNum = value;
        this.nodeNum.text = value.ToString();
    }

    public void UpdateNodeImage(Sprite sprite)
    {this.roomTypeImage.sprite = sprite;}
    public void MovePlayer()
    {DungeonManager.Inst.MovePlayer(roomNum);}

}