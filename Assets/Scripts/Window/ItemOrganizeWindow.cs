using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ItemOrganizeWindow : Window
{
	public static ItemOrganizeWindow Inst { get; private set; }
	public GameObject smallItemPrefab;

	public GridLayoutGroup gridLayout;

	public List<GameObject> itemObjectList;

	public void SetItemList(List<ItemData> itemList)
	{
		foreach (ItemData value in itemList)
		{
			GameObject inventoryItemObject = Instantiate(smallItemPrefab, new Vector3(0, 0, 0), Utils.QI);
			itemObjectList.Add(inventoryItemObject);

			inventoryItemObject.GetComponent<DeckCard>().Init(
			(deckCard, eventData) => {
			}
			, // 클릭 시
			(deckCard, eventData) => {

			} // 마우스 입장
			,
			(deckCard, eventData) => {

			} // 마우스 퇴장
			,
			(deckCard, eventData) => {
			}, // 드래그 시작
			(deckCard, eventData) => {
			}, // 드래그 중
			(deckCard, eventData) => {
			} // 드래그 끝

		);

			inventoryItemObject.transform.SetParent(gridLayout.transform);
			inventoryItemObject.transform.localScale = Vector3.one;
			inventoryItemObject.GetComponent<DeckCard>().SetItem(value);
		}
	}
}