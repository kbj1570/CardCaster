public class Node
{
    public ERoomType roomType;
    public Node()
    {this.roomType = ERoomType.None;}

    public ERoomType GetRoomType()
    {return roomType;}
    public void SetRoomType(ERoomType value)
    {this.roomType = value;}
}