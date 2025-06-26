using System;
using System.Collections.Generic;

public abstract class RandomEvent
{
	protected string eventName;//돌발상황 이름
	protected string eventNum;//돌발상황 고유번호
	protected List<string> eventDesc; //돌발상황 내용

	public int resultValue = 0;

	protected Dictionary<string, EventNode> eventNodes;
	
	public string GetName()
	{return eventName;}
	public string GetNum()
	{return eventNum;}
	public string GetDesc()
	{return eventDesc;}
	public Dictionary<string, EventNode> GetEventNodes()
	{ return eventNodes; }

	public abstract string GetResult();

}
public enum ERequireType
{None, EGold, EItem, EHealth, ECard }
public enum EEventEffectType
{ None, EGainItem, EGainGold, EAddValue}

public class EventNode
{
	public string id;
	public List<string> desc;
	public List<EventSelection> eventSelections;
}

public class EventSelection
{
	public string text;
	public List<string> resultText;
	public string nextNodeId;
	public ERequireType requireType;
	public string requireValue;
	public EEventEffectType effect;
	public string effectValue;
}
