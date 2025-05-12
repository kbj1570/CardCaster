using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScenes
{
	protected List<CutSceneNode> cutSceneNodes;
	public List<CutSceneNode> GetCutSceneNodes()
	{return cutSceneNodes;}

}

public class CutSceneNode
{
	public ECutSceneCommand cutSceneCommand;
	public string text;
	public string name;
	public int valueNum;
	public float waitTime;
	public string characterName;
	public CardData cardData;
	public Item item;
}

public enum ECutSceneCommand
{Wait,
ShowText,
HideText,
ShowCharacterLeftSide,
ShowCharacterCenter,
ShowCharacterRightSide,
HideCharacterLeftSide,
HideCharacterCenter,
HideCharacterRightSide,
HighLightCharacterLeftSide,
HighLightCharacterCenter,
HighLightCharacterRightSide,
ShakeScreen,
FadeInScreen,
FadeOutScreen,
PlaySoundEffect,
PlayMusic,

HideTextBox,
GainItem,
GainCard
}