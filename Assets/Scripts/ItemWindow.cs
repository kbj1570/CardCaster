using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemWindow : Window
{
	public Transform itemLocationParent;
	private List<Transform> itemLocations;
	private List<GameObject> itemObjectList;
	public List<Sprite> itemSpriteList;

	public GameObject itemPrefab;

	public Toggle toolToggle;
	public Toggle othersToggle;

	List<Item> toolList;
	Dictionary<Item, int> othersList;

	List<Item> itemDatabase;

	public GameObject itemDescriptionWindow;
	public GameObject itemUsingAlert;

	public bool itemLocked;

	public static ItemWindow Inst { get; private set; }

	void Start()
	{
		itemDatabase = DataController.Inst.LoadItemDatabase();
		itemObjectList = new();
		othersList = new();
		toolList = new();
		ScaleZero();
	}
	void Update()
	{
		
	}

	void Awake()
	{
		Inst = this;
		itemLocations = new List<Transform>();
		foreach (Transform child in itemLocationParent)
		{itemLocations.Add(child);}
	}

	private void LoadItemList()
	{
		toolList.Clear();
		othersList.Clear();

		foreach (KeyValuePair<string, int> value in PlayerData.saveData.others)
		{ othersList.Add(itemDatabase[Int32.Parse(value.Key)], value.Value);}

		foreach (string value in PlayerData.saveData.items)
		{ toolList.Add(itemDatabase[Int32.Parse(value)]);}
	}

	public void UpdateItemPage()
	{

		foreach (GameObject gameObject in itemObjectList)
		{ Destroy(gameObject); }

		LoadItemList();

		EItemCategory selectedItemCategory = EItemCategory.ETool;

		if (toolToggle.isOn)
		{ selectedItemCategory = EItemCategory.ETool; }
		else if (othersToggle.isOn)
		{ selectedItemCategory = EItemCategory.EOthers; }

		int index = 0;

		if (selectedItemCategory == EItemCategory.ETool)
		{
			foreach(Item item in toolList)
			{

				GameObject itemObject = Instantiate(itemPrefab, Vector3.zero, Utils.QI);
				itemObject.transform.localScale = Vector3.one;

				itemObject.GetComponent<DungeonItem>().SetUp(item, itemSpriteList[Int32.Parse(item.GetNum())]);

				itemObject.transform.SetParent(itemLocations[index]);
				itemObject.transform.localScale = Vector3.one;
				itemObject.transform.localPosition = Vector3.zero;
				itemObjectList.Add(itemObject);

				index++;
			}

		}
		else if (selectedItemCategory == EItemCategory.EOthers)
		{
			foreach (KeyValuePair<Item, int> itemPair in othersList)
			{
				GameObject itemObject = Instantiate(itemPrefab, Vector3.zero, Utils.QI);
				itemObject.transform.localScale = Vector3.one;

				itemObject.GetComponent<DungeonItem>().SetUp(itemPair.Key, itemPair.Value, itemSpriteList[Int32.Parse(itemPair.Key.GetNum())]);

				itemObject.transform.SetParent(itemLocations[index]);
				itemObject.transform.localScale = Vector3.one;
				itemObject.transform.localPosition = Vector3.zero;
				itemObjectList.Add(itemObject);

				index++;
			}
		}


		//foreach (KeyValuePair<Item, int> itemPair in myItemList)
		//	{
		//		if (itemPair.Key.GetItemCategory() == selectedItemCategory)
		//		{
		//			GameObject itemObject = Instantiate(itemPrefabList[0], new Vector3(0, 0, 0), Utils.QI);

		//			itemObject.GetComponent<DungeonItem>().SetUp(itemPair.Key, itemPair.Value,
		//			itemImageList[Int32.Parse(itemPair.Key.GetNum())]);

		//			itemObject.transform.SetParent(verticalItemScroll);
		//			itemObjectList.Add(itemObject);
		//		}
		//	}
	}

	public void ShowItemDescription(int itemNum)
	{
		itemDescriptionWindow.SetActive(true);
		itemDescriptionWindow.GetComponent<DescriptionWindow>().SetUp(itemDatabase[itemNum].GetName(),
		itemDatabase[itemNum].GetItemDescription());
	}

	public void HideItemDescription()
	{ itemDescriptionWindow.SetActive(false); }

	public void SelectUsingItem(Item item)
	{
		if(!itemLocked)
		{
			itemUsingAlert.GetComponent<Window>().OnOff();
			itemUsingAlert.GetComponent<ItemAlert>().SetText(item.GetName());
		}
	}
}
