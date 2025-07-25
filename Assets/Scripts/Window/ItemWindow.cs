using System;
using System.Collections.Generic;
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

		foreach (KeyValuePair<string, int> value in PlayerData.saveData.inventory_others)
		{ othersList.Add(itemDatabase[Int32.Parse(value.Key)], value.Value);}

		foreach (string value in PlayerData.saveData.inventory_items)
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

				}, // 드래그 시작
				(itemSlot, eventData) => {
					itemSlot.transform.position = Input.mousePosition;
				}, // 드래그 중
				(itemSlot, eventData) => {
					itemSlot.transform.localPosition = Vector3.zero;
				} // 드래그 끝

				);

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

				}, // 드래그 시작
				(itemSlot, eventData) => {
					itemSlot.transform.position = Input.mousePosition;
				}, // 드래그 중
				(itemSlot, eventData) => {
					itemSlot.transform.localPosition = Vector3.zero;
				} // 드래그 끝

			);




				itemObject.transform.SetParent(itemLocations[index]);
				itemObject.transform.localScale = Vector3.one;
				itemObject.transform.localPosition = Vector3.zero;
				itemObjectList.Add(itemObject);

				index++;
			}
		}
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
