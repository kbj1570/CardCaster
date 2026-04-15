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


	public  GridLayoutGroup gridLayout;

	public GameObject scrollView;
	public GameObject storageItemPrefab;

	private List<GameObject> itemObjectList;
	private List<GameObject> storageItemObjectList;
	public List<Sprite> itemSpriteList;

	public Dictionary<ItemData, int> itemStorageList;

	public GameObject itemPrefab;

	List<ItemData> inventoryToolList;
	List<ItemData> storageToolList;


	List<ItemData> itemDatabase;

	public GameObject itemDescriptionWindow;
	public GameObject itemUsingAlert;

	public GameObject storageArea;
	public GameObject inventoryArea;
	private Vector3 velocity = Vector3.zero;
	public bool itemLocked;

	public static StorageWindow Inst { get; private set; }

	void Start()
	{
		itemDatabase = DataController.Inst.LoadItemDatabase();
		itemObjectList = new();
		inventoryToolList = new();
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
		storageToolList.Clear();



		foreach (string value in PlayerData.saveData.inventory_items)
		{ inventoryToolList.Add(itemDatabase[Int32.Parse(value)]);}


		foreach (string value in PlayerData.saveData.storage_items)
		{ storageToolList.Add(itemDatabase[Int32.Parse(value)]); }
	}

	public void UpdateItemPage()
	{

		foreach (GameObject gameObject in itemObjectList)
		{ Destroy(gameObject); }

		foreach (GameObject gameObject in storageItemObjectList)
		{ Destroy(gameObject); }

		LoadItemList();

		EItemCategory selectedItemCategory = EItemCategory.ETool;



		int index = 0;

		foreach (ItemData item in inventoryToolList)
		{

			GameObject itemObject = Instantiate(itemPrefab, Vector3.zero, Utils.QI);
			itemObject.transform.localScale = Vector3.one;

			itemObject.GetComponent<Item>().SetUp(item, itemSpriteList[Int32.Parse(item.GetNum())]);

			itemObject.GetComponent<Item>().Init(
				(item, eventData) => {
					//ShowItemDescription(Int32.Parse(item.GetItem().GetNum()));
				}
			, // Ŭ�� ��
			(itemSlot, eventData) => {
				//ShowItemDescription(Int32.Parse(itemSlot.GetItem().GetNum()));
			} // ���콺 ����
			,
			(itemSlot, eventData) => {
				//HideItemDescription();
			} // ���콺 ����
			,
			(itemSlot, eventData) => {
				storageArea.SetActive(true);
				inventoryArea.SetActive(true);
				inventoryArea.transform.SetSiblingIndex(transform.childCount - 1);
				storageArea.transform.SetSiblingIndex(transform.childCount - 2);
				itemLocationParent.transform.SetSiblingIndex(transform.childCount - 3);
			}, // �巡�� ����
			(itemSlot, eventData) => {
				//itemSlot.transform.position = Input.mousePosition;
				itemSlot.transform.position = Vector3.SmoothDamp(itemSlot.transform.position, Input.mousePosition, ref velocity, 0.015f);
			}, // �巡�� ��
			(itemSlot, eventData) => {
				if (mouseOnArea == EMouseOnArea.Storage)
				{
					PlayerData.saveData.inventory_items.Remove(itemSlot.GetItem().GetNum());
					StoreItem(itemSlot.GetItem());
					UpdateItemPage();
				}
				else if (mouseOnArea == EMouseOnArea.Inventory)
				{itemSlot.transform.localPosition = Vector3.zero;}
				else
				{itemSlot.transform.localPosition = Vector3.zero;}


				storageArea.SetActive(false);
				inventoryArea.SetActive(false);
				ResetMouseOnArea();
			} // �巡�� ��

			);

			itemObject.transform.SetParent(invnetoryLocations[index]);
			itemObject.transform.localScale = Vector3.one;
			itemObject.transform.localPosition = Vector3.zero;
			itemObjectList.Add(itemObject);

			index++;
		}

		foreach (ItemData value in storageToolList)
		{
			GameObject inventoryItemObject = Instantiate(storageItemPrefab, new Vector3(0, 0, 0), Utils.QI);
			storageItemObjectList.Add(inventoryItemObject);

			inventoryItemObject.GetComponent<DeckCard>().Init(
			(deckCard, eventData) => {

			}
			, // Ŭ�� ��
			(deckCard, eventData) => {

			} // ���콺 ����
			,
			(deckCard, eventData) => {

			} // ���콺 ����
			,
			(deckCard, eventData) => {
				storageArea.SetActive(true);
				inventoryArea.SetActive(true);
				deckCard.transform.SetParent(transform);

				storageArea.transform.SetSiblingIndex(transform.childCount - 1);
				inventoryArea.transform.SetSiblingIndex(transform.childCount - 2);
				deckCard.transform.SetSiblingIndex(transform.childCount - 3);
			}, // �巡�� ����
			(deckCard, eventData) => {
				deckCard.transform.position = Vector3.SmoothDamp(deckCard.transform.position, Input.mousePosition, ref velocity, 0.015f);
			}, // �巡�� ��
			(deckCard, eventData) => {
				if (mouseOnArea == EMouseOnArea.Storage)
				{
					UpdateItemPage();
				}
				else if (mouseOnArea == EMouseOnArea.Inventory && inventoryToolList.Count < 9)
				{
					PlayerData.saveData.storage_items.Remove(deckCard.GetItem().GetNum());
					PlayerData.saveData.inventory_items.Add(deckCard.GetItem().GetNum());
					UpdateItemPage();
				}
				else
				{ UpdateItemPage(); }

				storageArea.SetActive(false);
				inventoryArea.SetActive(false);
				ResetMouseOnArea();
			}

		);

			inventoryItemObject.transform.SetParent(gridLayout.transform);
			inventoryItemObject.transform.localScale = Vector3.one;
			inventoryItemObject.GetComponent<DeckCard>().SetItem(value);
		}



	}

	public void StoreItem(ItemData item)
	{
		PlayerData.saveData.storage_items.Add(item.GetNum());}

	public void ShowItemDescription(int itemNum)
	{
		itemDescriptionWindow.SetActive(true);
		itemDescriptionWindow.GetComponent<DescriptionWindow>().SetUp(itemDatabase[itemNum].GetName(),
		itemDatabase[itemNum].GetItemDescription());
	}

	public void HideItemDescription()
	{ itemDescriptionWindow.SetActive(false); }

	public void SelectUsingItem(ItemData item)
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
