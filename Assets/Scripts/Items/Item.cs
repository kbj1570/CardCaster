public class Item
{
    protected string itemName;//아이템의 이름
    protected string itemNum;//아이템의 고유번호
    protected string itemDescription; //아이템의 효과 설명
    protected string itemInfo; // 아이템의 도감설명
    protected EItemCategory itemCategory;//아이템 분류
    
    public string GetName()
    {return itemName;}

    public string GetNum()
    {return itemNum;}

    public string GetItemDescription()
    {return itemDescription;}

}
public enum EItemCategory
{ETool, EUsableItem, EImportantItem, EUnUsableItem}