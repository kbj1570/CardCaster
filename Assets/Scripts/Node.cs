public class Node
{
    private ERoomType roomType;
    private int gold;
    private ItemData item;

    private int dialogueNum;

    private Enemy enemy;


    public Node()
    {this.roomType = ERoomType.None;}

    public ERoomType GetRoomType()
    {return roomType;}
    public void SetRoomType(ERoomType value)
    {this.roomType = value;}

    public ItemData GetItem()
    {return item;}

    public int GetGold()
    {return gold;}

    public void SetItem(ItemData item)
    {
        this.item = item;
    }

    public int GetDialogueNum() { return dialogueNum; }
    public void SetDialogueNum(int dialogueNum) { this.dialogueNum = dialogueNum; }


	public void SetGold(int value)
    {this.gold = value;}

    public void SetEnemy(Enemy enemy)
    {this.enemy = enemy;}

    public Enemy GetEnemy()
    {return enemy;}
}