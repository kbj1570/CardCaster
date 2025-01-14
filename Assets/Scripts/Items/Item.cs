public class Item
{
    public string itemName;//아이템의 이름
    public string itemNum;//아이템의 고유번호
    public string itemDescription; //아이템의 효과 설명
    public string itemInfo; // 아이템의 도감설명
    public EItemCategory itemCategory;//아이템 분류
    
    public string GetName()
    {return itemName;}

    public string GetNum()
    {return itemName;}

}
public enum EItemCategory
{ETool, EUsableItem, EImportantItem, EUnUsableItem}