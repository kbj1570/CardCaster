using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemWindow : Window
{
	public Transform itemLocationParent;
	private List<Transform> itemLocations;
	private List<GameObject> itemObjectList;
	public List<Sprite> itemSpriteList;

	public GameObject itemPrefab;

	public Toggle toolToggle;

	List<ItemData> toolList;
	Dictionary<ItemData, int> othersList;

	List<ItemData> itemDatabase;

	public GameObject itemDescriptionWindow;
	public GameObject itemUsingAlert;

	public TMP_Text shardCount;
	public TMP_Text goldCount;


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

		foreach (string value in PlayerData.saveData.inventory_items)
		{ toolList.Add(itemDatabase[Int32.Parse(value)]);}

		goldCount.text = PlayerData.saveData.gold.ToString();
		shardCount.text = PlayerData.saveData.shard.ToString();
	}

	public void UpdateItemPage()
	{
		foreach (GameObject gameObject in itemObjectList)
		{ Destroy(gameObject); }

		LoadItemList();



		int index = 0;

		foreach (ItemData item in toolList)
		{

			GameObject itemObject = Instantiate(itemPrefab, Vector3.zero, Utils.QI);
			itemObject.transform.localScale = Vector3.one;

			itemObject.GetComponent<Item>().SetUp(item, itemSpriteList[Int32.Parse(item.GetNum())]);

			itemObject.GetComponent<Item>().Init(
			(item, eventData) => {
				//ShowItemDescription(Int32.Parse(item.GetItem().GetNum()));
			}
			, // 클릭 시
			(itemSlot, eventData) => {
				//ShowItemDescription(Int32.Parse(itemSlot.GetItem().GetNum()));
			} // 마우스 입장
			,
			(itemSlot, eventData) => {
				//HideItemDescription();
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
}
