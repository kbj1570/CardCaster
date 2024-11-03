using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Dictionary<ItemSO, int> usableItemDic = new Dictionary<ItemSO, int>();
    private Dictionary<ItemSO, int> unusableItemDic = new Dictionary<ItemSO, int>();
    private Dictionary<ItemSO, int> keyItemDic = new Dictionary<ItemSO, int>();
    public static ItemManager Inst {get; private set;}
    void Awake() => Inst = this;

    public ItemManager()
    {

    }

    public Dictionary<ItemSO, int> GetUsableItemData(){return usableItemDic;}
    public Dictionary<ItemSO, int> GetUnUsableItemData(){return unusableItemDic;}
    public Dictionary<ItemSO, int> GetKeyItemData(){return keyItemDic;}
}
