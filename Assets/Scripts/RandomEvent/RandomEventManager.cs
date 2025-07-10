using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
	RandomEvent randomEvent;
	public TMP_Text eventNameText;
	public TMP_Text eventDescText;

	public GameObject selectionButton_0;
	public GameObject selectionButton_1;
	public GameObject selectionButton_2;
	public GameObject selectionButton_3;

	public GameObject endButton;


	public TMP_Text selectionText_0;
	public TMP_Text selectionText_1;
	public TMP_Text selectionText_2;
	public TMP_Text selectionText_3;

	EventNode currentNode;
	List<EventSelection> currentEventSelections;


	Dictionary<string, EventNode> eventNodes;


	void Start()
	{

	}

	void Awake()
	{
		randomEvent = new FunnyCookingTime();
		Initiate();
	}




	public void Initiate()
	{
		eventNameText.text = randomEvent.GetName();
		eventDescText.text = randomEvent.GetDesc()[0];
		eventNodes = randomEvent.GetEventNodes();

		StartCoroutine(Load("node_00"));
	}

	public IEnumerator Load(string nodeNum)
	{

		selectionButton_0.SetActive(false);
		selectionButton_1.SetActive(false);
		selectionButton_2.SetActive(false);
		selectionButton_3.SetActive(false);

		endButton.SetActive(false);

		currentNode = eventNodes[nodeNum];


		currentEventSelections = currentNode.eventSelections;
		//eventDescText.text = currentNode.desc[0];

		float typingSpeed = 0f;

		for (int p = 0; p < currentNode.desc.Count; ++p)
		{
			for (int q = 0; q < currentNode.desc[p].Length; q++)
			{
				if (currentNode.desc[p][q] == '.' ||
				currentNode.desc[p][q] == '!' ||
				currentNode.desc[p][q] == '?')
				{ typingSpeed = 0.17f; }
				else
				{ typingSpeed = 0.05f; }
				eventDescText.text = currentNode.desc[p].Substring(0, q + 1);
				yield return new WaitForSeconds(typingSpeed);

			}
		}

		

		if (currentEventSelections == null)
		{
			eventDescText.text = randomEvent.GetResult();
			endButton.SetActive(true);
		}
		else
		{
			switch (currentEventSelections.Count)
			{
				case 0:
					endButton.SetActive(true);
					break;

				case 1:
					selectionButton_0.SetActive(true);
					selectionText_0.text = currentEventSelections[0].text;
					break;

				case 2:
					selectionButton_0.SetActive(true);
					selectionButton_1.SetActive(true);

					selectionText_0.text = currentEventSelections[0].text;
					selectionText_1.text = currentEventSelections[1].text;
					break;

				case 3:
					selectionButton_0.SetActive(true);
					selectionButton_1.SetActive(true);
					selectionButton_2.SetActive(true);

					selectionText_0.text = currentEventSelections[0].text;
					selectionText_1.text = currentEventSelections[1].text;
					selectionText_2.text = currentEventSelections[2].text;
					break;

				case 4:
					selectionButton_0.SetActive(true);
					selectionButton_1.SetActive(true);
					selectionButton_2.SetActive(true);
					selectionButton_3.SetActive(true);

					selectionText_0.text = currentEventSelections[0].text;
					selectionText_1.text = currentEventSelections[1].text;
					selectionText_2.text = currentEventSelections[2].text;
					selectionText_3.text = currentEventSelections[3].text;
					break;
			}
		}
	}

	public void LoadNextNode(int nodeOrder)
	{
	
		switch(currentEventSelections[nodeOrder].effect)
		{
			case EEventEffectType.None:
				break;

			case EEventEffectType.EGainItem:
				break;

			case EEventEffectType.EGainGold:
				break;

			case EEventEffectType.EAddValue:

				int value = Int32.Parse(currentEventSelections[nodeOrder].effectValue);

				randomEvent.resultValue += value ;


				break;
		}
		StartCoroutine(Load(currentEventSelections[nodeOrder].nextNodeId));
	}

	public void EndRandomEvent()
	{
		Debug.Log("이벤트 종료");
	}
}