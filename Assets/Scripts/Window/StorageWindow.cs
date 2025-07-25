using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class StorageWindow : Window
{

	private EMouseOnArea mouseOnArea;
	public Transform itemLocationParent;
	public Transform storageLocationParent;
	private List<Transform> invnetoryLocations;
	private List<Transform> storageLocations;

	public  GridLayoutGroup gridLayout;

	public GameObject storageItemPrefab;

	private List<GameObject> itemObjectList;
	private List<GameObject> storageItemObjectList;
	public List<Sprite> itemSpriteList;

	public Dictionary<Item, int> itemStorageList;

	public GameObject itemPrefab;
	public GameObject smallItemPrefab;

	public Toggle toolToggle;
	public Toggle othersToggle;

	List<Item> inventoryToolList;
	Dictionary<Item, int> inventoryOthersList;

	List<Item> storageToolList;
	Dictionary<Item, int> storageOthersList;


	List<Item> itemDatabase;

	public GameObject itemDescriptionWindow;
	public GameObject itemUsingAlert;

	public GameObject storageArea;
	public GameObject inventoryArea;

	public bool itemLocked;

	public static StorageWindow Inst { get; private set; }

	void Start()
	{
		itemDatabase = DataController.Inst.LoadItemDatabase();
		itemObjectList = new();
		inventoryOthersList = new();
		inventoryToolList = new();
		storageOthersList = new();
		storageToolList = new();
		storageItemObjectList = new();

		mouseOnArea = EMouseOnArea.None;
		ScaleZero();
	}
	void Update()
	{
		
	}

	void Awake()
	{
		Inst = this;
		invnetoryLocations = new List<Transform>();
		foreach (Transform child in itemLocationParent)
		{invnetoryLocations.Add(child);}
	}

	private void LoadItemList()
	{
		inventoryToolList.Clear();
		inventoryOthersList.Clear();
		storageOthersList.Clear();
		storageToolList.Clear();

		foreach (KeyValuePair<string, int> value in PlayerData.saveData.inventory_others)
		{ inventoryOthersList.Add(itemDatabase[Int32.Parse(value.Key)], value.Value);}

		foreach (string value in PlayerData.saveData.inventory_items)
		{ inventoryToolList.Add(itemDatabase[Int32.Parse(value)]);}

		foreach (KeyValuePair<string, int> value in PlayerData.saveData.storage_others)
		{ storageOthersList.Add(itemDatabase[Int32.Parse(value.Key)], value.Value); }

		foreach (string value in PlayerData.saveData.storage_items)
		{ storageToolList.Add(itemDatabase[Int32.Parse(value)]); }
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
			foreach(Item item in inventoryToolList)
			{

				GameObject itemObject = Instantiate(itemPrefab, Vector3.zero, Utils.QI);
				itemObject.transform.localScale = Vector3.one;

				itemObject.GetComponent<DungeonItem>().SetUp(item, itemSpriteList[Int32.Parse(item.GetNum())]);

				itemObject.GetComponent<DungeonItem>().Init(
					(item, eventData) => {
						ShowItemDescription(Int32.Parse(item.GetItem().GetNum()));
					}
				, // 클릭 시
				(itemSlot, eventData) => {
					//ShowItemDescription(Int32.Parse(itemSlot.GetItem().GetNum()));
				} // 마우스 입장
				,
				(itemSlot, eventData) => {
					HideItemDescription();
				} // 마우스 퇴장
				,
				(itemSlot, eventData) => {
					storageArea.SetActive(true);
					inventoryArea.SetActive(true);
				}, // 드래그 시작
				(itemSlot, eventData) => {
					itemSlot.transform.position = Input.mousePosition;
				}, // 드래그 중
				(itemSlot, eventData) => {

					if(mouseOnArea == EMouseOnArea.Storage)
					{
						PlayerData.saveData.inventory_items.Remove(itemSlot.GetItem().GetNum());
						StoreItem(itemSlot.GetItem());
						UpdateItemPage();
					}
					else if(mouseOnArea == EMouseOnArea.Inventory)
					{
						itemSlot.transform.localPosition = Vector3.zero;
					}
					else
					{
						itemSlot.transform.localPosition = Vector3.zero;
					}

						
					storageArea.SetActive(false);
					inventoryArea.SetActive(false);
					ResetMouseOnArea();
				} // 드래그 끝

				);

				itemObject.transform.SetParent(invnetoryLocations[index]);
				itemObject.transform.localScale = Vector3.one;
				itemObject.transform.localPosition = Vector3.zero;
				itemObjectList.Add(itemObject);

				index++;
			}

			foreach (Item value in storageToolList)
			{
				GameObject inventoryItemObject = Instantiate(storageItemPrefab, new Vector3(0, 0, 0), Utils.QI);
				storageItemObjectList.Add(inventoryItemObject);

				inventoryItemObject.transform.SetParent(gridLayout.transform);
				//gameObject.GetComponent<DeckCard>().SetCard(value.Key, value.Value);
			}

		}
		else if (selectedItemCategory == EItemCategory.EOthers)
		{
			foreach (KeyValuePair<Item, int> itemPair in inventoryOthersList)
			{
				GameObject itemObject = Instantiate(itemPrefab, Vector3.zero, Utils.QI);
				itemObject.transform.localScale = Vector3.one;

				itemObject.GetComponent<DungeonItem>().SetUp(itemPair.Key, itemPair.Value, itemSpriteList[Int32.Parse(itemPair.Key.GetNum())]);

				itemObject.GetComponent<DungeonItem>().Init(
				(item, eventData) => {
					ShowItemDescription(Int32.Parse(item.GetItem().GetNum()));
				}
				, // 클릭 시
				(item, eventData) => {
					//ShowItemDescription(Int32.Parse(item.GetItem().GetNum()));
				} // 마우스 입장
				,
				(item, eventData) => {
					HideItemDescription();
				} // 마우스 퇴장
				,
				(item, eventData) => {
					storageArea.SetActive(true);
					inventoryArea.SetActive(true);
				}, // 드래그 시작
				(itemSlot, eventData) => {
					itemSlot.transform.position = Input.mousePosition;
				}, // 드래그 중
				(itemSlot, eventData) => {
					if (mouseOnArea == EMouseOnArea.Storage)
					{
						PlayerData.saveData.inventory_others.Remove(itemSlot.GetItem().GetNum());
						StoreItem(itemSlot.GetItem());
						UpdateItemPage();
					}
					else if (mouseOnArea == EMouseOnArea.Inventory)
					{
						itemSlot.transform.localPosition = Vector3.zero;
					}
					else
					{
						itemSlot.transform.localPosition = Vector3.zero;
					}
					storageArea.SetActive(false);
					inventoryArea.SetActive(false);

					ResetMouseOnArea();
				} // 드래그 끝

			);

			foreach (KeyValuePair<Item, int> value in storageOthersList)
			{
				GameObject storageItemObject = Instantiate(storageItemPrefab, new Vector3(0, 0, 0), Utils.QI);
				storageItemObjectList.Add(storageItemObject);

				storageItemObject.transform.SetParent(gridLayout.transform);
				//gameObject.GetComponent<DeckCard>().SetCard(value.Key, value.Value);
			}




				itemObject.transform.SetParent(invnetoryLocations[index]);
				itemObject.transform.localScale = Vector3.one;
				itemObject.transform.localPosition = Vector3.zero;
				itemObjectList.Add(itemObject);

				index++;
			}
		}


		
	}

	public void StoreItem(Item item)
	{
		PlayerData.saveData.storage_items.Add(item.GetNum());
	}

	public void StoreItem(Item item, int count)
	{
		storageOthersList.Add(item, count);
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

	public void SetMouseOnArea(EMouseOnArea mouseOnArea)
	{this.mouseOnArea = mouseOnArea;}

	public void ResetMouseOnArea()
	{this.mouseOnArea = EMouseOnArea.None;}
}
