using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Node
{
    // private Node northNode;
    // private Node southNode;
    // private Node westNode;
    // private Node eastNode;
    // public Boolean isCreated;
    // public int connectCount;

    public ERoomType roomType;
    public Node()
    {this.roomType = ERoomType.None;}

    public ERoomType GetRoomType()
    {return roomType;}
    public void SetRoomType(ERoomType value)
    {this.roomType = value;}

    // public Node GetNorthNode()
    // {return northNode;}
    // public Node GetSouthNode()
    // {return southNode;}
    // public Node GetWestNode()
    // {return westNode;}
    // public Node GetEastNode()
    // {return eastNode;}
    // public Boolean GetCreated()
    // {return isCreated;}
    // public void SetNorthNode(Node value)
    // {this.northNode = value;}
    // public void SetSouthNode(Node value)
    // {this.southNode = value;}
    // public void SetWestNode(Node value)
    // {this.westNode = value;}
    // public void SetEastNode(Node value)
    // {this.eastNode = value;}
    // public void SetCreated(Boolean value)
    // {this.isCreated = value;}
    // public int GetConnectCount()
    // {
    //     int value = 4;
    //     if(northNode == null)
    //     {--value;}
    //     if(eastNode == null)
    //     {--value;}

    //     if(westNode == null)
    //     {--value;}

    //     if(southNode == null)
    //     {--value;}

    //     return value;
    // }
}