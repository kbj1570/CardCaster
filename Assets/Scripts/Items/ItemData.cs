using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private string itemName;//아이템의 이름
    [SerializeField]
    private string itemNum;//아이템의 고유번호
    [SerializeField]
    private string itemDescription; //아이템의 효과 설명
    [SerializeField]
    private string itemInfo; // 아이템의 도감설명
    [SerializeField]
    private EItemCategory itemCategory;//아이템 분류
    
    public string GetName()
    {return itemName;}

    public string GetNum()
    {return itemNum;}

    public string GetItemDescription()
    {return itemDescription;}

    public EItemCategory GetItemCategory()
    {return itemCategory;}

}
public enum EItemCategory
{ETool, EDocument, EImportantItem}