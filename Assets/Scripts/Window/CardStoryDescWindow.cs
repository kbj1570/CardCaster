using TMPro;
using UnityEngine;

public class CardStoryDescWindow : Window
{
    CardData cardData;

    public TMP_Text cardNameText;
	public TMP_Text cardDescText;
	public TMP_Text cardStoryDescText;
	public TMP_Text cardTypeText;

	public Transform cardLocation;


	public GameObject dummyServentCardPrefab;
	public GameObject dummySpellCardPrefab;



	public void SetCardData(CardData cardData)
    {
        this.cardData = cardData;
        cardNameText.text = cardData.GetCardName();
        cardDescText.text = cardData.GetCardDesc();
        cardStoryDescText.text = cardData.GetCardStoryDesc();



        switch(cardData.GetCardType())
        {
            case ECardType.Servent:
                cardTypeText.text = "Servent";
				break;

            case ECardType.Spell:
				cardTypeText.text = "Spell";
				break;
		}

		GameObject selectedCardPrefab = null;

		switch (cardData.GetCardType())
		{
			case ECardType.Servent:
				selectedCardPrefab = dummyServentCardPrefab;
				break;
			case ECardType.Spell:
				selectedCardPrefab = dummySpellCardPrefab;
				break;

		}

		GameObject focusOnCard = Instantiate(selectedCardPrefab,
		new Vector3(0, 0, 0), Utils.QI);

		focusOnCard.GetComponent<Card>().SetCard(cardData);

		focusOnCard.transform.SetParent(cardLocation);
		focusOnCard.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		focusOnCard.transform.localPosition = new Vector3(0, 0, 0);
	}

	public void CloseWindow()
	{
		Destroy(cardLocation.GetChild(0).gameObject);

		this.cardData = null;
		cardNameText.text = "";
		cardDescText.text = "";
		cardStoryDescText.text = "";

		OnOff();
	}
}
