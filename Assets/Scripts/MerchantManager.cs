//using TMPro;
//using UnityEngine;

//public class MerchantWindow : MonoBehaviour
//{
//	public MerchantItemSlot[] itemSlots;
//	public Item[] itemsForSale; // 6개 아이템
//	public PlayerInventory playerInventory;
//	public GameObject confirmWindow;
//	public TMP_Text messageText; // 돈 부족 메시지 출력용

//	private void Start()
//	{
//		for (int i = 0; i < itemSlots.Length; i++)
//		{
//			if (i < itemsForSale.Length)
//				itemSlots[i].Setup(itemsForSale[i], this);
//		}
//		messageText.text = "";
//	}

//	public void TryBuyItem(Item item)
//	{
//		confirmWindow.Open(item, () =>
//		{
//			if (playerInventory.TrySpendGold(item.price))
//			{
//				playerInventory.AddItem(item);
//				messageText.text = $"{item.itemName} 구매 완료!";
//			}
//			else
//			{
//				messageText.text = "돈이 부족합니다!";
//			}
//		});
//	}
//}