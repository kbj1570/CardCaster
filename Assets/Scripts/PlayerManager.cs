using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    Player player;
    public EItemCategory selectedCategory;
    public int additionalHealth;
    public int maxHealth;

    private int health;

    public List<ItemSO> inventory;
    public Dictionary<ItemSO, int> importantItem;
    public Dictionary<ItemSO, int> usableItem;
    public Dictionary<ItemSO, int> unusableItem;

    public Dictionary<CardData, int> myCardList;

    public GridLayoutGroup gridLayout;
    public TMP_Text goldText;
    public GameObject itemPrefab;

    public List<GameObject> itemObjectList;

    public static PlayerManager Inst{get; private set;}

    void Awake()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;
        DontDestroyOnLoad(this);
    }


    void Start()
    {
        // importantItem = new Dictionary<ItemSO, int>();
        // usableItem = new Dictionary<ItemSO, int>();
        // unusableItem = new Dictionary<ItemSO, int>();
        // selectedCategory = EItemCategory.EImportantItem;
        // ClassifyItems();
        // ShowInventory();

    }
    
    public void ClassifyItems()
    {
        goldText.text = player.GetGold().ToString();

        importantItem.Clear();
        usableItem.Clear();
        unusableItem.Clear();

        foreach(ItemSO item in inventory)
        {
            switch(item.GetItemClass())
            {
                case EItemClass.Important:
                if(!importantItem.ContainsKey(item))
                {importantItem.Add(item, 1);}
                else
                {importantItem[item]++;}
                break;

                case EItemClass.Usable:
                if(!usableItem.ContainsKey(item))
                {usableItem.Add(item, 1);}
                else
                {usableItem[item]++;}
                break;

                case EItemClass.Unusable:
                if(!unusableItem.ContainsKey(item))
                {unusableItem.Add(item, 1);}
                else
                {unusableItem[item]++;}
                break;
            }
        }
    }

    public void ShowInventory()
    {
        foreach(GameObject value in itemObjectList)
        {
            Destroy(value);
        }
        itemObjectList.Clear();

        Dictionary<ItemSO, int> dictionary = null;
        switch(selectedCategory)
        {
            case EItemCategory.EImportantItem:
            dictionary = importantItem;
            break;

            case EItemCategory.EUsableItem:
            dictionary = usableItem;
            break;

            case EItemCategory.EUnUsableItem:
            dictionary = unusableItem;
            break;
        }

        foreach(KeyValuePair<ItemSO, int> value in dictionary)
        {
            GameObject gameObject = Instantiate(itemPrefab, new Vector3() , Utils.QI);
            gameObject.transform.SetParent(gridLayout.transform);
            gameObject.GetComponent<InventoryItem>().SetItemData(value.Key);
            gameObject.GetComponent<InventoryItem>().SetItemCount(value.Value);

            itemObjectList.Add(gameObject);
        }
    }

    public void SetCategory(int value)
    {
        EItemCategory itemCategory = EItemCategory.ETool;
        switch(value)
        {
            case 0:
            itemCategory = EItemCategory.EImportantItem;
            break;

            case 1:
            itemCategory = EItemCategory.EUsableItem;
            break;

            case 2:
            itemCategory = EItemCategory.EUnUsableItem;
            break;
        }
        this.selectedCategory = itemCategory;
    }


    public void AddItem(ItemSO value, int count)
    {

        for(int i = 0; i < count; ++i)
        {inventory.Add(value);}

        ClassifyItems();
    }

    public void GainGold(int value)
    {
        player.SetGold(player.GetGold() + value);
        
        ClassifyItems();
    }

    public void LoseItem(ItemSO value, int count)
    {     
        for(int i = 0; i < count; ++i)
        {inventory.Remove(value);}

        ClassifyItems();
    }

    public void LoseGold(int value)
    {
        player.SetGold(player.GetGold() - value);

        ClassifyItems();
    }

    public void AddAdditionalHealth(int value)
    {additionalHealth += value;}

    public Dictionary<ItemSO, int> GetSellItem()
    {

        Dictionary<ItemSO, int> itemData = new Dictionary<ItemSO, int>();

        foreach(KeyValuePair<ItemSO, int> value in usableItem)
        {itemData.Add(value.Key, value.Value);}

        foreach(KeyValuePair<ItemSO, int> value in unusableItem)
        {itemData.Add(value.Key, value.Value);}

        return itemData;
    }


    public int GetHealth()
    {return health;}
    public void GainHealth(int value)
    {health += value;}

    public void LoseHealth(int value)
    {health -= value;}

    public void SetHealth(int value)
    {health = value;}

     public int GetGold()
    {return player.GetGold();}
}

