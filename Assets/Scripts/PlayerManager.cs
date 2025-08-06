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

    public SaveData saveData;

    public static PlayerManager Inst{get; private set;}

    void Awake()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;
		PlayerData.saveData = DataController.Inst.LoadData();
        DontDestroyOnLoad(this);
    }

}

