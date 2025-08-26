using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ItemOrganizeWindow : Window
{
	public static ItemOrganizeWindow Inst { get; private set; }
	public GameObject smallItemPrefab;

	public GridLayoutGroup gridLayout;

	public List<GameObject> itemObjectList;

	public void SetGold(int gold)
	{
		GameObject inventoryItemObject = Instantiate(smallItemPrefab, new Vector3(0, 0, 0), Utils.QI);
		itemObjectList.Add(inventoryItemObject);

		inventoryItemObject.GetComponent<DeckCard>().SetGold(gold);
		inventoryItemObject.GetComponent<DeckCard>().Init(
		(itemSlot, eventData) => {
			PlayerData.saveData.gold += itemSlot.GetGold();
			Destroy(itemSlot.gameObject);
		}
		, // 클릭 시
		(itemSlot, eventData) => {

		} // 마우스 입장
		,
		(itemSlot, eventData) => {

		} // 마우스 퇴장
		,
		(itemSlot, eventData) => {
		}, // 드래그 시작
		(itemSlot, eventData) => {
		}, // 드래그 중
		(itemSlot, eventData) => {
		} // 드래그 끝

	);

		inventoryItemObject.transform.SetParent(gridLayout.transform);
		inventoryItemObject.transform.localScale = Vector3.one;
	}

	public void SetItemList(List<ItemData> itemList)
	{
		foreach (ItemData value in itemList)
		{
			GameObject inventoryItemObject = Instantiate(smallItemPrefab, new Vector3(0, 0, 0), Utils.QI);
			itemObjectList.Add(inventoryItemObject);

			inventoryItemObject.GetComponent<DeckCard>().Init(
			(itemSlot, eventData) => {
				if(itemSlot.GetItem().GetNum() == "0")
				{
					PlayerData.saveData.shard++;
					Destroy(itemSlot.gameObject);
				}
				else if(PlayerData.saveData.inventory_items.Count < 9)
				{
					PlayerData.saveData.inventory_items.Add(itemSlot.GetItem().GetNum());
					Destroy(itemSlot.gameObject);
				}
				else
				{
					BattleManager.Inst.AlertMessage("인벤토리가 가득 찼습니다.");
				}
				
			}
			, // 클릭 시
			(itemSlot, eventData) => {

			} // 마우스 입장
			,
			(itemSlot, eventData) => {

			} // 마우스 퇴장
			,
			(itemSlot, eventData) => {
			}, // 드래그 시작
			(itemSlot, eventData) => {
			}, // 드래그 중
			(itemSlot, eventData) => {
			} // 드래그 끝

		);

			inventoryItemObject.transform.SetParent(gridLayout.transform);
			inventoryItemObject.transform.localScale = Vector3.one;
			inventoryItemObject.GetComponent<DeckCard>().SetItem(value);
		}
	}
}