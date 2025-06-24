using System;
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


	public void Initiate()
	{
		eventNameText.text = randomEvent.GetName();
		eventDescText.text = randomEvent.GetDesc();

		eventNodes = randomEvent.GetEventNodes();
	}

	public void Load()
	{
		currentNode = eventNodes["node_01"];
		List<EventSelection> eventSelections = currentNode.eventSelections;



		switch (eventSelections.Count)
		{
			case 0:
				endButton.SetActive(true);
				break;

			case 1:
				selectionButton_0.SetActive(true);
				selectionText_0.text = eventSelections[0].text;
				break;

			case 2:
				selectionButton_0.SetActive(true);
				selectionButton_1.SetActive(true);

				selectionText_0.text = eventSelections[0].text;
				selectionText_1.text = eventSelections[1].text;
				break;

			case 3:
				selectionButton_0.SetActive(true);
				selectionButton_1.SetActive(true);
				selectionButton_2.SetActive(true);

				selectionText_0.text = eventSelections[0].text;
				selectionText_1.text = eventSelections[1].text;
				selectionText_2.text = eventSelections[2].text;
				break;

			case 4:
				selectionButton_0.SetActive(true);
				selectionButton_1.SetActive(true);
				selectionButton_2.SetActive(true);
				selectionButton_3.SetActive(true);

				selectionText_0.text = eventSelections[0].text;
				selectionText_1.text = eventSelections[1].text;
				selectionText_2.text = eventSelections[2].text;
				selectionText_3.text = eventSelections[3].text;
				break;
		}
	}

	public void LoadNextNode(string nodeNum)
	{
		currentNode = eventNodes[nodeNum];

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